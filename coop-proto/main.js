'use strict';
// Меню, ввод, игровой цикл.

const Input = (() => {
  const keys = new Set();
  let ePresses = 0; // счётчик нажатий E: по сети не теряется и не дублируется
  addEventListener('keydown', ev => {
    if (ev.target.tagName === 'INPUT') return;
    if (ev.code === 'KeyE' && !ev.repeat) ePresses++;
    keys.add(ev.code);
  });
  addEventListener('keyup', ev => keys.delete(ev.code));
  addEventListener('blur', () => keys.clear());
  const k = c => keys.has(c) ? 1 : 0;
  return {
    get: () => ({
      mx: (k('KeyD') || k('ArrowRight')) - (k('KeyA') || k('ArrowLeft')),
      my: (k('KeyS') || k('ArrowDown')) - (k('KeyW') || k('ArrowUp')),
      e: ePresses,
      r: keys.has('KeyR'),
    }),
  };
})();

// Таймер из Web Worker: в фоновой вкладке обычный setInterval/rAF у хоста тормозят до 1 Гц,
// а сообщения воркера приходят вовремя — симуляция не встаёт, когда хост переключил вкладку.
function makeTicker(ms, fn) {
  try {
    const src = 'setInterval(() => postMessage(0), ' + ms + ');';
    const w = new Worker(URL.createObjectURL(new Blob([src], { type: 'text/javascript' })));
    w.onmessage = fn;
    return () => w.terminate();
  } catch (e) {
    const id = setInterval(fn, ms);
    return () => clearInterval(id);
  }
}

const Game = {
  state: null, // состояние симуляции (только у хоста)
  myId: null,
  stopTicker: null,

  startHost() {
    const s = this.state = Sim.create();
    const me = Sim.addPlayer(s, 'local');
    this.myId = me.id;
    const dt = 1 / CONFIG.TICK_RATE;
    let last = performance.now(), acc = 0;
    this.stopTicker = makeTicker(1000 / CONFIG.TICK_RATE, () => {
      const now = performance.now();
      acc += Math.min(0.25, (now - last) / 1000);
      last = now;
      me.input = Input.get();
      while (acc >= dt) { Sim.step(s, dt); acc -= dt; }
    });
  },

  view() {
    return this.state;
  },
};

function frame() {
  const v = Game.view();
  if (v) Render.draw(v, Game.myId);
  requestAnimationFrame(frame);
}

document.getElementById('btn-solo').onclick = () => {
  document.getElementById('menu').hidden = true;
  Game.startHost();
};
requestAnimationFrame(frame);
