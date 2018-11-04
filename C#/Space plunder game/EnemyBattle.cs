using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SPP
{
    public class EnemyBattle : MonoBehaviour
    {
        static public int EnemyType;
        static public int m_enemyHealth;
        private int m_enemyStartHealth;
        [SerializeField]private int m_minDamage;
        [SerializeField]private int m_maxDamage;
        [SerializeField]private int m_Damage;

        [SerializeField]private float m_waitTime;

        [SerializeField]private bool m_enemySetup;
        [SerializeField]private bool m_enemyAttack;

        [SerializeField]private GameObject m_spacePirates;
        [SerializeField]private GameObject m_ghostShip;
        [SerializeField]private GameObject m_drone;
        [SerializeField]private GameObject m_theCube;
        [SerializeField]private GameObject m_spaceGuardian;
        [SerializeField]private GameObject m_enemyWeaponCurrently;
        [SerializeField]private GameObject m_PlayerBattle;

        [SerializeField]private GameObject[] m_enemyWeapon;

        [SerializeField]private Slider m_healthSlider;
        [SerializeField]private TextMeshProUGUI m_enemyHealthAmmountText;

        [SerializeField]private Animator m_GundamAnimator;

    
        // Use this for initialization
        void Start()
        {
           // m_PlayerBattle = GameObject.Find("PlayerBattle");
            m_enemySetup = false;
            m_spacePirates.SetActive(false);
            m_ghostShip.SetActive(false);
            m_drone.SetActive(false);
            m_theCube.SetActive(false);
            m_spaceGuardian.SetActive(false);
        }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            m_healthSlider.value = m_enemyHealth;
            m_enemyHealthAmmountText.text = ("HP:"+m_enemyHealth);
            if (EnemyType == 5)
            {
                if (m_enemyAttack == true)
                {
                    m_GundamAnimator.SetTrigger("Shoot");
                }
                else if (m_enemyHealth <= 0)
                {
                    m_GundamAnimator.SetTrigger("Dead");
                }
                else
                {
                    
                }
            }
            if (m_enemySetup == false && PlayerExploration.m_battleStarted==true)
            {
                print("switch enemy type: " + EnemyType);
                switch (EnemyType)
                {
                    case 1:
                        m_spacePirates.SetActive(true);
                        m_healthSlider.maxValue = 50;
                        m_enemyStartHealth = 50;
                        m_enemyHealth = 50;
                        m_enemySetup = true;
                        m_minDamage = 6;
                        m_maxDamage = 10;
                        m_enemyWeaponCurrently = m_enemyWeapon[0];
                        m_waitTime = 3.2f;
                        break;
                    case 2:
                        m_ghostShip.SetActive(true);
                        m_healthSlider.maxValue = 60;
                        m_enemyStartHealth = 60;
                        m_enemyHealth = 60;
                        m_enemySetup = true;
                        m_minDamage = 8;
                        m_maxDamage = 14;
                        m_enemyWeaponCurrently = m_enemyWeapon[1];
                        m_waitTime = 4f;
                        break;
                    case 3:
                        m_drone.SetActive(true);
                        m_healthSlider.maxValue = 100;
                        m_enemyStartHealth = 100;
                        m_enemyHealth = 100;
                        m_enemySetup = true;
                        m_minDamage = 8;
                        m_maxDamage = 12;
                        m_enemyWeaponCurrently = m_enemyWeapon[2];
                        m_waitTime = 2.2f;
                        break;
                    case 4:
                        m_theCube.SetActive(true);
                        m_healthSlider.maxValue = 70;
                        m_enemyStartHealth = 70;
                        m_enemyHealth = 70;
                        m_enemySetup = true;
                        m_minDamage = 10;
                        m_maxDamage = 16;
                        m_enemyWeaponCurrently = m_enemyWeapon[3];
                        m_waitTime = 2.6f;
                        break;
                    case 5:
                        m_spaceGuardian.SetActive(true);
                        m_healthSlider.maxValue = 80;
                        m_enemyStartHealth = 80;
                        m_enemyHealth = 80;
                        m_enemySetup = true;
                        m_minDamage = 10;
                        m_maxDamage = 20;
                        m_enemyWeaponCurrently = m_enemyWeapon[4];
                        m_waitTime = 4f;
                        break;
                    default:
                        break;
                }


            }

            if (PhazeManager.m_playerBattleTurn == false && m_enemyHealth <= 0 && m_enemySetup == true)
            {
                m_enemyHealth = 0;
                EnemyDefeatedInBattle();
            }

            if (PhazeManager.m_playerBattleTurn == false && m_enemyAttack == true&& m_enemyHealth > 0)
            {
                StartCoroutine (AttackWaitTime(m_enemyWeaponCurrently, m_waitTime));
                m_enemyAttack = false;
            }

        }
        void Attack()
        {
            m_Damage = Random.Range(m_minDamage, m_maxDamage);
            m_PlayerBattle.GetComponent<PlayerBattle>().TakeDamage(m_Damage,EnemyType);
            print("enemy damage: "+m_Damage);
        }

        public void EnemySetup(int type)
        {
            type = EnemyType;

            print("battle enemy type: " + EnemyType);
        }

        public void EnemyTakesDamage(int dmg, int weapontype)
        {              

            if (m_enemyHealth > 0)
            {
                if (weapontype == EnemyType)
                {
                    int temphitpoints = m_enemyHealth;
                    m_enemyHealth = temphitpoints - dmg;
                }
                else if (weapontype == (EnemyType-1))
                {
                    int temphitpoints = m_enemyHealth;
                    m_enemyHealth = temphitpoints - (dmg -10);
                }  
                else if (weapontype != EnemyType && weapontype !=3)
                {
                    int temphitpoints = m_enemyHealth;
                    m_enemyHealth = temphitpoints - (dmg / 2);
                }                
                else if (weapontype != EnemyType && weapontype == 3)
                {
                    int temphitpoints = m_enemyHealth;
                    m_enemyHealth = temphitpoints - (dmg / 4);
                }
            }

            m_enemyAttack = true;
        }


        public void EnemyDefeatedInBattle()
        {
            m_PlayerBattle.GetComponent<PlayerBattle>().EndBattlePhaze();
            m_enemySetup = false;
            m_spacePirates.SetActive(false);
            m_ghostShip.SetActive(false);
            m_drone.SetActive(false);
            m_theCube.SetActive(false);
            m_spaceGuardian.SetActive(false);
        }
        IEnumerator AttackWaitTime(GameObject weapon, float wait)
        {
            weapon.SetActive(true);
            yield return new WaitForSeconds(wait);
            Attack();
            weapon.SetActive(false);
            PhazeManager.m_playerBattleTurn = true;
        }
    }
}
