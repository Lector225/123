'use strict';
// Все числа баланса и сети. Крутить здесь.
// В мультиплеере действует CONFIG хоста: симуляция идёт только у него.
const CONFIG = {
  // Карта и сеть
  MAP_W: 2400,
  MAP_H: 2400,
  TICK_RATE: 60,            // тиков симуляции в секунду (только хост)
  SNAPSHOT_RATE: 20,        // снапшотов в секунду хост → клиенты
  INTERP_DELAY: 0.1,        // с, на сколько клиент рисует «в прошлом» для плавности
  MAX_PLAYERS: 4,

  // Игрок
  PLAYER_SPEED: 230,        // px/с
  PLAYER_RADIUS: 14,
  PLAYER_HP: 100,
  PLAYER_RANGE: 350,        // радиус автострельбы
  PLAYER_FIRE_INTERVAL: 0.2,
  PLAYER_DAMAGE: 10,
  RESPAWN_TIME: 5,          // с

  // База
  BASE_RADIUS: 60,
  BASE_HP: 1000,

  // Враги
  ENEMY_RADIUS: 12,
  ENEMY_SPEED: 75,
  ENEMY_HP: 30,
  ENEMY_DAMAGE: 6,
  ENEMY_ATTACK_INTERVAL: 0.8,
  ENEMY_AGGRO_RADIUS: 150,  // игроков ближе этого враг атакует вместо базы
  MAX_ENEMIES: 150,

  // Волны
  FIRST_WAVE_DELAY: 5,      // с до первой волны
  WAVE_INTERVAL: 25,        // с между волнами
  WAVE_BASE_COUNT: 8,       // врагов в волне n: BASE + GROWTH * (n - 1)
  WAVE_COUNT_GROWTH: 4,
  WAVE_PER_EXTRA_PLAYER: 0.5, // +50% врагов за каждого игрока сверх первого
  WAVE_HP_GROWTH: 0.1,      // +10% HP врагов за волну
  WAVE_SIDES_EVERY: 3,      // каждые N волн враги идут ещё с одной стороны
  SPAWN_RATE: 4,            // врагов в секунду, пока волна выходит
};
