'use strict';
// Симуляция. Работает только у хоста (и в одиночной игре); клиенты получают снапшоты.
const Sim = (() => {
  const C = CONFIG;
  const COLORS = ['#4fc3f7', '#ffb74d', '#aed581', '#f48fb1'];
  let nextId = 1;

  const d2 = (a, b) => (a.x - b.x) ** 2 + (a.y - b.y) ** 2;
  const clamp = (v, lo, hi) => Math.max(lo, Math.min(hi, v));
  const alive = t => t && !t.dead && t.hp > 0;

  function create() {
    return {
      time: 0, tick: 0, kills: 0, over: false,
      wave: 0, nextWaveAt: C.FIRST_WAVE_DELAY, waveSides: [0], waveHpMul: 1, spawnLeft: 0, spawnAcc: 0,
      base: { kind: 'base', x: C.MAP_W / 2, y: C.MAP_H / 2, r: C.BASE_RADIUS, hp: C.BASE_HP, maxHp: C.BASE_HP },
      players: [], enemies: [], tracers: [],
    };
  }

  function addPlayer(s, peer) {
    const used = new Set(s.players.map(p => p.slot));
    let slot = 0;
    while (used.has(slot)) slot++;
    if (slot >= C.MAX_PLAYERS) return null;
    const p = {
      kind: 'player', id: nextId++, peer, slot, label: 'P' + (slot + 1), color: COLORS[slot],
      x: 0, y: 0, r: C.PLAYER_RADIUS, hp: C.PLAYER_HP, maxHp: C.PLAYER_HP,
      dead: false, respawn: 0, cd: 0, input: { mx: 0, my: 0, e: 0, r: false },
    };
    placeAtBase(s, p);
    s.players.push(p);
    return p;
  }

  function removePlayer(s, peer) {
    s.players = s.players.filter(p => p.peer !== peer);
  }

  function placeAtBase(s, p) {
    const a = p.slot * Math.PI / 2 + Math.PI / 4;
    const d = s.base.r + 40;
    p.x = s.base.x + Math.cos(a) * d;
    p.y = s.base.y + Math.sin(a) * d;
  }

  // Выталкивает круг a из круга o.
  function pushOut(a, o) {
    const dx = a.x - o.x, dy = a.y - o.y, min = a.r + o.r, dd = dx * dx + dy * dy;
    if (dd >= min * min) return;
    const d = Math.sqrt(dd) || 0.01;
    a.x = o.x + dx / d * min;
    a.y = o.y + dy / d * min;
  }

  function damage(s, t, dmg) {
    t.hp -= dmg;
    if (t.hp > 0) return;
    t.hp = 0;
    if (t.kind === 'player') { t.dead = true; t.respawn = C.RESPAWN_TIME; }
  }

  // Хитскан по ближайшему врагу в радиусе. Трассер — только визуал.
  function shoot(s, from, range, dmg, color) {
    let best = null, bd = range * range;
    for (const e of s.enemies) {
      if (e.hp <= 0) continue;
      const d = d2(from, e);
      if (d < bd) { bd = d; best = e; }
    }
    if (!best) return false;
    best.hp -= dmg;
    if (best.hp <= 0) s.kills++;
    s.tracers.push({ x1: from.x, y1: from.y, x2: best.x, y2: best.y, c: color, life: 0.08, tick: s.tick });
    return true;
  }

  function updatePlayer(s, p, dt) {
    if (p.dead) {
      p.respawn -= dt;
      if (p.respawn <= 0) { p.dead = false; p.hp = p.maxHp; placeAtBase(s, p); }
      return;
    }
    let { mx, my } = p.input;
    const len = Math.hypot(mx, my);
    if (len > 1) { mx /= len; my /= len; }
    const sp = C.PLAYER_SPEED;
    p.x = clamp(p.x + mx * sp * dt, p.r, C.MAP_W - p.r);
    p.y = clamp(p.y + my * sp * dt, p.r, C.MAP_H - p.r);
    pushOut(p, s.base);

    p.cd -= dt;
    if (p.cd <= 0 && shoot(s, p, C.PLAYER_RANGE, C.PLAYER_DAMAGE, p.color)) p.cd = C.PLAYER_FIRE_INTERVAL;
  }

  // Цель: ближайший живой игрок в радиусе агрессии, иначе база.
  function pickTarget(s, e) {
    let best = null, bd = C.ENEMY_AGGRO_RADIUS ** 2;
    for (const p of s.players) {
      if (p.dead) continue;
      const d = d2(e, p);
      if (d < bd) { bd = d; best = p; }
    }
    return best || s.base;
  }

  function updateEnemy(s, e, dt) {
    if (e.hp <= 0) return;
    e.think -= dt;
    if (e.think <= 0 || !alive(e.target)) {
      e.think = 0.3 + Math.random() * 0.2;
      e.target = pickTarget(s, e);
    }
    const t = e.target;
    const dx = t.x - e.x, dy = t.y - e.y, dist = Math.hypot(dx, dy) || 0.01;
    e.cd -= dt;
    if (dist > t.r + e.r + 4) {
      const v = C.ENEMY_SPEED * dt / dist;
      e.x += dx * v;
      e.y += dy * v;
    } else if (e.cd <= 0) {
      e.cd = C.ENEMY_ATTACK_INTERVAL;
      damage(s, t, C.ENEMY_DAMAGE);
    }
  }

  // Враги не налезают друг на друга и на базу. 150² / 2 пар — терпимо.
  function separate(s) {
    const es = s.enemies, n = es.length;
    for (let i = 0; i < n; i++) {
      const a = es[i];
      for (let j = i + 1; j < n; j++) {
        const b = es[j];
        const dx = b.x - a.x, dy = b.y - a.y, min = a.r + b.r, dd = dx * dx + dy * dy;
        if (dd >= min * min || dd < 1e-4) continue;
        const d = Math.sqrt(dd), k = (min - d) / d * 0.5;
        a.x -= dx * k; a.y -= dy * k;
        b.x += dx * k; b.y += dy * k;
      }
      pushOut(a, s.base);
    }
  }

  function spawnEnemy(s) {
    const side = s.waveSides[Math.floor(Math.random() * s.waveSides.length)];
    const m = 20, t = m + Math.random() * (C.MAP_W - 2 * m);
    const pos = [[t, m], [C.MAP_W - m, t], [t, C.MAP_H - m], [m, t]][side];
    const hp = Math.round(C.ENEMY_HP * s.waveHpMul);
    s.enemies.push({
      kind: 'enemy', id: nextId++, x: pos[0], y: pos[1], r: C.ENEMY_RADIUS,
      hp, maxHp: hp, cd: 0, think: 0, target: null,
    });
  }

  function updateWaves(s, dt) {
    if (s.time >= s.nextWaveAt) {
      s.wave++;
      s.nextWaveAt += C.WAVE_INTERVAL;
      const mul = 1 + C.WAVE_PER_EXTRA_PLAYER * Math.max(0, s.players.length - 1);
      s.spawnLeft += Math.round((C.WAVE_BASE_COUNT + C.WAVE_COUNT_GROWTH * (s.wave - 1)) * mul);
      s.waveHpMul = 1 + C.WAVE_HP_GROWTH * (s.wave - 1);
      const sides = [0, 1, 2, 3].sort(() => Math.random() - 0.5);
      s.waveSides = sides.slice(0, Math.min(4, 1 + Math.floor((s.wave - 1) / C.WAVE_SIDES_EVERY)));
    }
    s.spawnAcc = Math.min(s.spawnAcc + C.SPAWN_RATE * dt, 1);
    while (s.spawnLeft > 0 && s.spawnAcc >= 1 && s.enemies.length < C.MAX_ENEMIES) {
      s.spawnAcc -= 1;
      s.spawnLeft--;
      spawnEnemy(s);
    }
  }

  function step(s, dt) {
    if (s.over) return;
    s.time += dt;
    s.tick++;
    updateWaves(s, dt);
    for (const p of s.players) updatePlayer(s, p, dt);
    for (const e of s.enemies) updateEnemy(s, e, dt);
    separate(s);
    s.enemies = s.enemies.filter(e => e.hp > 0);
    for (const t of s.tracers) t.life -= dt;
    s.tracers = s.tracers.filter(t => t.life > 0);
    if (s.base.hp <= 0) s.over = true;
  }

  return { create, addPlayer, removePlayer, step };
})();
