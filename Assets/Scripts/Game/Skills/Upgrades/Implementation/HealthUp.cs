using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HealthUp : MonoBehaviour
{
        public CharacterHealthComponent characterHealthComponent; // Ссылка на компонент здоровья
        public int upgradeAmount = 20; // Количество увеличения здоровья

        void Start()
        {
            if (characterHealthComponent != null)
            {
                characterHealthComponent.IncreaseMaxHealth(upgradeAmount); // Увеличиваем здоровье
            }
        }
    }