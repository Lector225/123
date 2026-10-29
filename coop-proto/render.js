'use strict';
// Отрисовка. Принимает «вид» — объект той же формы, что состояние симуляции
// (у хоста это само состояние, у клиента — интерполированный снапшот).
const Render = (() => {
  const C = CONFIG;
  const cv = document.getElementById('game');
  const ctx = cv.getContext('2d');
  const cam = { x: C.MAP_W / 2, y: C.MAP_H / 2 };
  let W = 0, H = 0, dpr = 1;

  function resize() {
    dpr = window.devicePixelRatio || 1;
    W = innerWidth; H = innerHeight;
    cv.width = W * dpr; cv.height = H * dpr;
  }
  addEventListener('resize', resize);
  resize();

  function bar(x, y, w, frac, color) {
    ctx.fillStyle = '#000a';
    ctx.fillRect(x - w / 2 - 1, y - 1, w + 2, 6);
    ctx.fillStyle = color;
    ctx.fillRect(x - w / 2, y, w * Math.max(0, frac), 4);
  }

  function fmtTime(t) {
    const m = Math.floor(t / 60), s = Math.floor(t % 60);
    return m + ':' + String(s).padStart(2, '0');
  }

  function drawGrid() {
    const step = 100;
    const x0 = Math.max(0, Math.floor((cam.x - W / 2) / step) * step), x1 = Math.min(C.MAP_W, cam.x + W / 2);
    const y0 = Math.max(0, Math.floor((cam.y - H / 2) / step) * step), y1 = Math.min(C.MAP_H, cam.y + H / 2);
    ctx.strokeStyle = '#1f2633';
    ctx.lineWidth = 1;
    ctx.beginPath();
    for (let x = x0; x <= x1; x += step) { ctx.moveTo(x, y0); ctx.lineTo(x, y1); }
    for (let y = y0; y <= y1; y += step) { ctx.moveTo(x0, y); ctx.lineTo(x1, y); }
    ctx.stroke();
    ctx.strokeStyle = '#c62828';
    ctx.lineWidth = 4;
    ctx.strokeRect(0, 0, C.MAP_W, C.MAP_H);
  }

  function draw(v, myId) {
    const me = v.players.find(p => p.id === myId);
    if (me) { cam.x += (me.x - cam.x) * 0.25; cam.y += (me.y - cam.y) * 0.25; }

    ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
    ctx.fillStyle = '#12161f';
    ctx.fillRect(0, 0, W, H);
    ctx.setTransform(dpr, 0, 0, dpr, (W / 2 - cam.x) * dpr, (H / 2 - cam.y) * dpr);
    drawGrid();

    // База
    const b = v.base;
    ctx.fillStyle = '#37474f';
    ctx.strokeStyle = '#90a4ae';
    ctx.lineWidth = 3;
    ctx.beginPath(); ctx.arc(b.x, b.y, b.r, 0, Math.PI * 2); ctx.fill(); ctx.stroke();
    bar(b.x, b.y - b.r - 14, 120, b.hp / b.maxHp, '#66bb6a');

    // Враги — одним путём
    ctx.fillStyle = '#e53935';
    ctx.beginPath();
    for (const e of v.enemies) { ctx.moveTo(e.x + e.r, e.y); ctx.arc(e.x, e.y, e.r, 0, Math.PI * 2); }
    ctx.fill();
    for (const e of v.enemies) if (e.hp < e.maxHp) bar(e.x, e.y - e.r - 8, 22, e.hp / e.maxHp, '#ff8a80');

    // Трассеры
    ctx.lineWidth = 2;
    for (const t of v.tracers) {
      ctx.strokeStyle = t.c;
      ctx.globalAlpha = Math.min(1, t.life / 0.08);
      ctx.beginPath(); ctx.moveTo(t.x1, t.y1); ctx.lineTo(t.x2, t.y2); ctx.stroke();
    }
    ctx.globalAlpha = 1;

    // Игроки
    ctx.font = 'bold 12px sans-serif';
    ctx.textAlign = 'center';
    for (const p of v.players) {
      ctx.globalAlpha = p.dead ? 0.3 : 1;
      ctx.fillStyle = p.color;
      ctx.beginPath(); ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2); ctx.fill();
      if (p.id === myId) { ctx.strokeStyle = '#fff'; ctx.lineWidth = 2; ctx.stroke(); }
      ctx.fillStyle = '#fff';
      ctx.fillText(p.dead ? p.label + ' ' + Math.ceil(p.respawn) : p.label, p.x, p.y - p.r - 12);
      if (!p.dead) bar(p.x, p.y - p.r - 9, 30, p.hp / p.maxHp, '#4caf50');
    }
    ctx.globalAlpha = 1;

    // HUD
    ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
    ctx.textAlign = 'left';
    ctx.font = 'bold 16px sans-serif';
    ctx.fillStyle = '#fff';
    ctx.fillText('⏱ ' + fmtTime(v.time) + '   Волна ' + v.wave + '   Врагов ' + v.enemies.length + '   Убито ' + v.kills, 12, 24);
    ctx.fillText('База: ' + Math.ceil(b.hp) + ' / ' + b.maxHp, 12, 46);
    if (me && me.dead) {
      ctx.textAlign = 'center';
      ctx.font = 'bold 28px sans-serif';
      ctx.fillText('Возрождение через ' + Math.ceil(me.respawn), W / 2, H / 2 - 60);
    }
  }

  return { draw, fmtTime };
})();
