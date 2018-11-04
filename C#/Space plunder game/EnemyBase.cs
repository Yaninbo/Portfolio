using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SPP
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField]private int m_enemyType;
        [SerializeField]private GameObject m_enemyBlock;
        [SerializeField]private GameObject m_enemyModel;
        [SerializeField]private GameObject m_enemyFow;
        [SerializeField]private GameObject m_playerExploration;
        [SerializeField]private GameObject m_playerReference;
        public bool m_enemyFound;
        public bool m_fowRemoved;
        public bool m_LRRisOn = false;
        public bool m_enemyDestroyed;

        public Animator m_anim;

    
    
        // Use this for initialization
        void Awake()
        {
            m_anim = GetComponent<Animator>();
            m_LRRisOn = false;
            m_enemyFound = false;
            m_fowRemoved = false;
            m_enemyFow.SetActive(true);
            m_enemyModel.SetActive(false);
        }
        void Start()
        {
            m_playerReference = GameObject.Find("PlayerReference");
            m_playerExploration =GameObject.Find("PlayerExploration");
        }
        void FixedUpdate()
        {
            if (m_LRRisOn)
            {
                StartCoroutine(AfterLRR());
            }
        }

        public void Fow()
        {
            if (m_fowRemoved == false && m_enemyDestroyed == false)
            {
                m_playerExploration.GetComponent<PlayerExploration>().Enemy(m_enemyType);
                PlayerExploration.m_battleStarted = true;
                PhazeManager.m_playerBattleTurn = true;
                m_enemyModel.SetActive(true);
                m_enemyFow.SetActive(false);
                m_fowRemoved = true;
                StartCoroutine(StartBattle());
            }
            else if (m_fowRemoved == true && m_enemyDestroyed == true)
            {
                m_playerExploration.GetComponent<PlayerExploration>().MoveAfterBattle();

            }
        }
        IEnumerator StartBattle()
        {
            EnemyBattle.EnemyType = m_enemyType;
            yield return new WaitForSeconds(0.3f);
            m_playerReference.GetComponent<PhazeManager>().TransitionToBattle();
        }
        public void EnemyDefeated()
        {
            print("enemy defeated");
            m_enemyModel.SetActive(false);
            m_enemyDestroyed = true;
            m_playerExploration.GetComponent<PlayerExploration>().MoveAfterBattle();
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("LRR")&&m_fowRemoved ==false)
            {
                m_enemyFow.SetActive(false);
                m_enemyModel.SetActive(true);
                m_LRRisOn = true;
            }

        }

        public void OnTriggerStay (Collider other)
        {
            if(other.gameObject.CompareTag("SRR"))
            {
                Blink();
            }
        }

        public void Blink()
        {
            if (m_fowRemoved == false)
            {
                m_anim.Play("EnemyFowBlink");
            }
            else
            {
                return;
            }

        }
        IEnumerator AfterLRR()
        {
            yield return new WaitForSeconds(2f);
            m_enemyModel.SetActive(false);
            m_enemyFow.SetActive(true);
            m_LRRisOn = false;
        }
    }
}
