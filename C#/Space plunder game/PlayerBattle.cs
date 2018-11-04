using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SPP
{
    public class PlayerBattle : MonoBehaviour
    {
        
        [SerializeField]private int m_weaponType;
        [SerializeField]private int m_minDamage;
        [SerializeField]private int m_maxDamage;
        [SerializeField]private int m_weaponDmg;
        [SerializeField]private GameObject m_shield;
        [SerializeField]private GameObject m_playerReference;
        [SerializeField]private GameObject m_playerExploration;
        [SerializeField]private GameObject m_enemyBattle;
        public GameObject m_weaponwheel;
        public bool m_shieldsActivated = false;
        private bool m_weaponFireing = false;

        //Animation stuff
        public Animator m_battleAnimator;


        //Audio stuff
        public AudioSource m_battleSoundSource;

        [SerializeField]private float m_highPitchAudio;
        [SerializeField]private float m_lowPitchAudio;

        [SerializeField]private GameObject m_lazer;
        [SerializeField]private GameObject m_particleCannon;
        [SerializeField]private GameObject m_drillRam;
        [SerializeField]private GameObject m_photonTorpedo;
        [SerializeField]private GameObject m_nuke;
        [SerializeField]private GameObject m_meteorSling;    
        [SerializeField]private AudioClip m_takeDamageAudio;
        [SerializeField]private AudioClip m_takeDamageWithShieldAudio;
    
        // Use this for initialization
        void Start()
        {
            m_lazer.SetActive (false);
            m_particleCannon.SetActive (false);
            m_drillRam.SetActive (false);
            m_photonTorpedo.SetActive (false);
            m_nuke.SetActive (false);
            m_meteorSling.SetActive (false);    
        }
        
        // Update is called once per frame
        void Update()
        {

        }

        public void ShootLazer()
        {
            
            if (PhazeManager.m_playerBattleTurn == true)
            {
                m_minDamage = 20;
                m_maxDamage = 30;
                m_weaponDmg = Random.Range(m_minDamage, m_maxDamage);

                if (PlayerReference.m_PlayerShield > 0)
                {
                    BattleHud.m_message = "Shield durability down by 4";
                    m_weaponwheel.SetActive(false);
                    m_lazer.SetActive(true);
                    print(m_weaponDmg);
                    StartCoroutine(ShotAnimationWait(m_lazer, 2f, m_weaponDmg, 0));
                    PlayerReference.m_PlayerShield -= 4;
                    if (PlayerReference.m_PlayerShield <= 0)
                    {
                        ShieldActivation();
                    }
                }
                else if(PlayerReference.m_PlayerShield <= 0)
                {
                    BattleHud.m_message = "Not enough enrgy. This damages ship.";
                    m_weaponwheel.SetActive(false);
                    m_lazer.SetActive(true);
                    print(m_weaponDmg);
                    StartCoroutine(ShotAnimationWait(m_lazer, 2f, m_weaponDmg, 0));
                    PlayerReference.m_PlayerHealth -= 5;
                    if (PlayerReference.m_PlayerHealth <= 0)
                    {
                        TakeDamage(1, 7);
                    }
                }
            }
            else
            {
                return;
            }

        }

        public void ShootParticleCannon()
        {
            m_minDamage = 20;
            m_maxDamage = 40;
            m_weaponDmg = Random.Range(m_minDamage, m_maxDamage);

            if (PlayerReference.m_PlayerWeapon1 > 0 && PhazeManager.m_playerBattleTurn == true)
            {
                m_weaponwheel.SetActive(false);
                m_particleCannon.SetActive (true);
                PlayerReference.m_PlayerWeapon1 -= 1;
                print(m_weaponDmg);
                StartCoroutine(ShotAnimationWait(m_particleCannon,4f,m_weaponDmg,1));

            }
            else if(PlayerReference.m_PlayerWeapon1 <= 0)
            {
                BattleHud.m_message = "Out of Particles try something else";
            }
            else if(PhazeManager.m_playerBattleTurn == false)
            {
                return;
            }
                
        }
        public void ShootPhotonTorpedos()
        {
            m_minDamage = 30;
            m_maxDamage = 40;
            m_weaponDmg = Random.Range(m_minDamage, m_maxDamage);

            if (PlayerReference.m_PlayerWeapon2 > 0 && PhazeManager.m_playerBattleTurn == true)
            {
                m_weaponwheel.SetActive(false);
                m_photonTorpedo.SetActive (true);
                PlayerReference.m_PlayerWeapon2 -= 1;
                print(m_weaponDmg);
                StartCoroutine(ShotAnimationWait(m_photonTorpedo,1f,m_weaponDmg,2));
            }
            else if(PlayerReference.m_PlayerWeapon2 <= 0)
            {
                BattleHud.m_message = "Out of Photonium try something else";
            }
            else if(PhazeManager.m_playerBattleTurn == false)
            {
                return;
            }
        }
        public void ShootDrillRam()
        {
            m_minDamage = 60;
            m_maxDamage = 80;
            m_weaponDmg = Random.Range(m_minDamage, m_maxDamage);

            if (PlayerReference.m_PlayerWeapon3 > 0 && PhazeManager.m_playerBattleTurn == true)
            {
                m_weaponwheel.SetActive(false);
                m_drillRam.SetActive (true);
                PlayerReference.m_PlayerWeapon3 -= 1;
                print(m_weaponDmg);
                StartCoroutine(ShotAnimationWait(m_drillRam,5f,m_weaponDmg,3));
            }
            else if(PlayerReference.m_PlayerWeapon3 <= 0)
            {
                BattleHud.m_message = "Out of Golem cores try something else";
            }
            else if(PhazeManager.m_playerBattleTurn == false)
            {
                return;
            }
        }
        public void ShootNuke()
        {
            m_minDamage = 30;
            m_maxDamage = 40;
            m_weaponDmg = Random.Range(m_minDamage, m_maxDamage);

            if (PlayerReference.m_PlayerWeapon4 > 0 && PhazeManager.m_playerBattleTurn == true)
            {
                m_weaponwheel.SetActive(false);
                m_nuke.SetActive (true);
                PlayerReference.m_PlayerWeapon4 -= 1;
                print(m_weaponDmg);
                StartCoroutine(ShotAnimationWait(m_nuke,3f,m_weaponDmg,4));
            }
            else if(PlayerReference.m_PlayerWeapon1 <= 0)
            {
                BattleHud.m_message = "Out of yellowcake try something else";
            }
            else if(PhazeManager.m_playerBattleTurn == false)
            {
                return;
            }
        }
        public void ShootMeteorSling()
        {
            m_minDamage = 30;
            m_maxDamage = 40;
            m_weaponDmg = Random.Range(m_minDamage, m_maxDamage);

            if (PlayerReference.m_PlayerWeapon5 > 0 && PhazeManager.m_playerBattleTurn == true)
            {
                m_weaponwheel.SetActive(false);
                m_meteorSling.SetActive (true); 
                PlayerReference.m_PlayerWeapon5 -= 1;
                print(m_weaponDmg);
                StartCoroutine(ShotAnimationWait(m_meteorSling,3f,m_weaponDmg,5));
            }
            else if(PlayerReference.m_PlayerWeapon5 <= 0)
            {
                BattleHud.m_message = "No meteors nearby something else";
            }
            else if(PhazeManager.m_playerBattleTurn == false)
            {
                return;
            }
        }
        public void ShieldActivation()
        {
            if (PlayerReference.m_PlayerShield > 0)
            {
                m_shieldsActivated = !m_shieldsActivated;
                m_shield.SetActive(m_shieldsActivated);
            }
            else
            {
                PlayerReference.m_PlayerShield = 0;
                m_shieldsActivated = false;
                m_shield.SetActive(false);
            }
        }
        public void TakeDamage(int dmg,int enemy)
        {
            m_weaponwheel.SetActive(true);
            if (PhazeManager.m_playerBattleTurn == false && PlayerExploration.m_battleStarted == true)
            {
                PhazeManager.m_playerBattleTurn = true;
                if (PlayerReference.m_PlayerShield > 0 && PlayerReference.m_PlayerHealth > 0 && m_shieldsActivated == true)
                {
                    //player has health and shields and has chosen to use shields.
                    //shields block 50% of incoming damage
                    PlayerReference.m_PlayerHealth -= dmg / 2;
                    PlayerReference.m_PlayerShield -= dmg / 2;
                    if (PlayerReference.m_PlayerShield <= 0)
                    {
                        ShieldActivation();
                    }
                    if (PlayerReference.m_PlayerHealth <= 0)
                    {
                        m_playerReference.GetComponent<PlayerReference>().GameOver(enemy);
                    }
                    else
                    {
                        return;
                    }
                
                }
                else if ((PlayerReference.m_PlayerShield == 0 && PlayerReference.m_PlayerHealth > 0) || (m_shieldsActivated == false && PlayerReference.m_PlayerHealth > 0))
                {
                    //player dosen't have shields or does not to wish to use them.
                    PlayerReference.m_PlayerHealth -= dmg;
                    if (PlayerReference.m_PlayerHealth <= 0)
                    {
                        m_playerReference.GetComponent<PlayerReference>().GameOver(enemy);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        public void EndBattlePhaze()
        {
            print("Battle Ended");
            m_weaponwheel.SetActive(true);
            m_playerReference.GetComponent<PhazeManager>().TransitionToExploration();
            m_playerExploration.GetComponent<PlayerExploration>().EndBattle();
            //Enemy death animation
            //animated exploration transition
            //Change phaze from battle to exploration
            //move
        }

        IEnumerator ShotAnimationWait (GameObject weapon, float wait, int weaponDMG, int weaponNUM)
        {
            yield return new WaitForSeconds(wait);
            PhazeManager.m_playerBattleTurn = false;
            m_enemyBattle.GetComponent<EnemyBattle>().EnemyTakesDamage(weaponDMG, weaponNUM);
            weapon.SetActive(false);
        }
    }
}
