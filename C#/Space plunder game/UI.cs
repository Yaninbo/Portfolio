using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SPP
{
    public class UI : MonoBehaviour
    {
        public GameObject m_playerStats;
        public Dropdown m_dropdown;
        public Toggle m_tuttoggle;
        public Toggle m_muteToggle;
        public GameObject m_playerRef;
        public int m_tutnum;
        public int m_tutType;
        static public bool m_Stats;
        private bool m_statsDone;

        [SerializeField]private TextMeshProUGUI m_weapon1UI;
        [SerializeField]private TextMeshProUGUI m_weapon2UI;
        [SerializeField]private TextMeshProUGUI m_weapon3UI;
        [SerializeField]private TextMeshProUGUI m_weapon4UI;
        [SerializeField]private TextMeshProUGUI m_weapon5UI;
        [SerializeField]private GameObject m_victoryPanel;
        [SerializeField]private GameObject m_gameoverPanel;


        [SerializeField]private GameObject m_tutorialPanel;
        [SerializeField]private GameObject m_optionsPanel;
        [SerializeField]private TextMeshProUGUI m_TutorialText;
        [SerializeField]private TextMeshProUGUI m_TutorialTitleText;
        [SerializeField]private TextMeshProUGUI m_DefeatText;
        [SerializeField]private Dropdown m_language;


        [SerializeField]private Image m_tutImage1;
        [SerializeField]private Image m_tutImage2;
        [SerializeField]private Image m_tutImage3;
        [SerializeField]private Image m_tutImage4;
        [SerializeField]private Image m_tutImage5;
        [SerializeField]private Image m_tutImage6;
        [SerializeField]private Image m_tutImage7;
        [SerializeField]private Image m_tutImage8;
        [SerializeField]private Image m_tutImage9;
        [SerializeField]private Image m_tutImage10;
        [SerializeField]private Image m_tutImage11;
        [SerializeField]private Image m_tutImage12;
        [SerializeField]private Image m_tutImage13;
        [SerializeField]private Image m_tutImage14;
        [SerializeField]private Image m_tutImage15;
        [SerializeField]private Image m_tutImage16;
        [SerializeField]private Image m_tutImage17;
        public GameObject[] m_englishTexts;
        public GameObject[] m_finnishTexts;


    
        void Start()
        {
            ToggleTutorial(PlayerPrefX.GetBool("Tutorial"));
            m_tuttoggle.isOn = PlayerPrefX.GetBool("Tutorial");
            m_muteToggle.isOn =  PlayerPrefX.GetBool("Mute");
            m_language.value = PlayerPrefs.GetInt("Language");
            LanguageText(PlayerPrefs.GetInt("Language"));
            m_optionsPanel.SetActive(false);
            Time.timeScale = 1f;
            m_victoryPanel.SetActive(false);
            m_gameoverPanel.SetActive(false);
            m_tutorialPanel.SetActive(false);
            m_statsDone = m_Stats;
            m_tutType = m_dropdown.value;
            m_tutImage1.enabled = false;
            m_tutImage2.enabled = false;
            m_tutImage3.enabled = false;
            m_tutImage4.enabled = false;
            m_tutImage5.enabled = false;
            m_tutImage6.enabled = false;
            m_tutImage7.enabled = false;
            m_tutImage8.enabled = false;
            m_tutImage9.enabled = false;
            m_tutImage10.enabled = false;
            m_tutImage11.enabled = false;
            m_tutImage12.enabled = false;
            m_tutImage13.enabled = false;
            m_tutImage14.enabled = false;
            m_tutImage15.enabled = false;
            m_tutImage16.enabled = false;
            m_tutImage17.enabled = false;
            Tutorial(9);
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            if (m_tutnum != 0)
            {
                Tutorial(m_tutnum);
            }
            if (PlayerExploration.m_battleStarted == false &&m_statsDone == true)
            {
                Invoke("ShowStats",2f);
                m_statsDone = false;
            }
            else if (PlayerExploration.m_battleStarted == true&& m_statsDone == false)
            {
                HideStats();
            }

            m_weapon1UI.text = PlayerReference.m_PlayerWeapon1.ToString();
            m_weapon2UI.text = PlayerReference.m_PlayerWeapon2.ToString();
            m_weapon3UI.text = PlayerReference.m_PlayerWeapon3.ToString();
            m_weapon4UI.text = PlayerReference.m_PlayerWeapon4.ToString();
            m_weapon5UI.text = PlayerReference.m_PlayerWeapon5.ToString();
        }
        void ShowStats()
        {
            m_playerStats.SetActive(true);
        }
        void HideStats()
        {
            m_playerStats.SetActive(false);
            m_statsDone = true;
        }
        public void CloseTutorial()
        {
            if (m_tutnum == 9)
            {
                m_tutnum = 0;
                m_tutImage5.enabled = false;
                Tutorial(8);

            }
            else
            {
                m_tutorialPanel.SetActive(false);
                m_tutImage1.enabled = false;
                m_tutImage2.enabled = false;
                m_tutImage3.enabled = false;
                m_tutImage4.enabled = false;
                m_tutImage5.enabled = false;
                m_tutImage6.enabled = false;
                m_tutImage7.enabled = false;
                m_tutImage8.enabled = false;
                m_tutImage9.enabled = false;
                m_tutImage10.enabled = false;
                m_tutImage11.enabled = false;
                m_tutImage12.enabled = false;
                m_tutImage13.enabled = false;
                m_tutImage14.enabled = false;
                m_tutImage15.enabled = false;
                m_tutImage16.enabled = false;
                m_tutImage17.enabled = false;
                Time.timeScale = 1F;
            }
        }
        public void ToggleTutorial(bool tutorial)
        {
            if (tutorial == true)
            {
                PlayerPrefX.SetBool("Tutorial",true);;
            }
            else if (tutorial == false)
            {
                PlayerPrefX.SetBool("Tutorial",false);;
                m_tutorialPanel.SetActive(false);
                Time.timeScale = 1F;
            }
            m_tutorialPanel.SetActive(false);
            Time.timeScale = 1F;
        }
        public void OptionsPanelOn()
        {
            m_optionsPanel.SetActive(true);
        }
        public void OptionsPanelOff()
        {
            m_optionsPanel.SetActive(false);
        }
        public void LanguageText(int language)
        {
            PlayerPrefs.SetInt("Language", language);

            if (PlayerPrefs.GetInt("Language")==0)
            {
                foreach (GameObject go in m_englishTexts)
                {
                    go.SetActive(true);
                }
                foreach (GameObject go in m_finnishTexts)
                {
                    go.SetActive(false);
                }
                m_language.value = PlayerPrefs.GetInt("Language");
            }
            else if (PlayerPrefs.GetInt("Language")==1)
            {

                foreach (GameObject go in m_englishTexts)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in m_finnishTexts)
                {
                    go.SetActive(true);
                }
                m_language.value = PlayerPrefs.GetInt("Language");
            }
        }
        public void Tutorial(int situation)
        {
            m_tutType = m_dropdown.value;
                
            if (PlayerPrefX.GetBool("Tutorial")==true && m_tutType == 0)
            {
                m_tutorialPanel.SetActive(true);

                switch (situation)
                {
                    case 1:
                        m_TutorialTitleText.text="Treasure Piece";
                        m_TutorialText.text = "We found the first Star of Sampo, only 8 stars to go Captain.";
                        m_tutImage5.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 2:
                        m_TutorialTitleText.text="Spacejunk";
                        m_TutorialText.text = "We can use parts of this blown up spacejunck to repair the ship a bit. 20 Hull points and 10 Shield points recovered.";
                        m_tutImage11.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 3:
                        m_TutorialTitleText.text="Enhanced radar area";
                        m_TutorialText.text = "Here we can get a better sweep of possible hazards, but just for a moment. I hope you can remember these Captain.";
                        m_tutImage6.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 4:
                        m_TutorialTitleText.text="Satellite";
                        m_TutorialText.text = "We can hook up into galctic satelite systems and get a reading of an local area. Please designate an area that you would like to see, by pressing once in the middle of it.";
                        m_tutImage9.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 5:
                        m_TutorialTitleText.text="Space map";
                        m_TutorialText.text = "We were able to secure a treasure star map, but its going to self-destruct after use so we'll see the location only briefly. The map seems to somehow locate the nearest star piece and it should show in the map briefly.";
                        m_tutImage8.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 6:
                        m_TutorialTitleText.text="Supply pod";
                        m_TutorialText.text = "Captain we retrived an abbandoned supply pod that was filled with ammunition.";
                        m_tutImage7.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 7:
                        m_TutorialTitleText.text="Asteroid field";
                        m_TutorialText.text = "Sorry Captain! We can't dodge all these asteroids, the ship is going to take some damage.";
                        m_tutImage12.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 8:
                        m_TutorialTitleText.text = "Movement";
                        m_TutorialText.text = "These arrows around the ship are the movement controlls. Tap the corresponding arrow to move in the chosen direction.";
                        m_tutImage1.enabled = true;
                        m_tutImage2.enabled = true;
                        m_tutImage3.enabled = true;
                        m_tutImage4.enabled = true;
                        break;
                    case 9:
                        m_TutorialTitleText.text = "Mission Statement";
                        m_TutorialText.text = "We have reason to belive that the legendary Stars of Sampo are scattered in this star system. This is a dangerous area and we have limited resourses. There are a lot of dangerous things lurking in the dark that we must try to avoid and be smart about if we will want to survive this.";
                        m_tutImage5.enabled = true;
                        m_tutnum = 9;
                        break;
                    case 10:
                        m_TutorialTitleText.text="Planets";
                        m_TutorialText.text = "We can gather some resources from these planets, but they're quite poor so we can buy repairs and ammo only once per planet. Restocking can take years this far off so multiple supply runs are just not possible. We have marked planets we haven't visited yet with a green tanker symbol. Visiting a planet resupplys us with 20 Hull points, 10 Shield points, 1 of each ammo type.";
                        m_tutImage10.enabled = true;
                        break;
                    case 11:
                        m_TutorialTitleText.text="Local Pirates";
                        m_TutorialText.text = "It's the scum of the space and they want to plunder us. Not anything our weapons can't handle. These ships can take quite a beating but rumors say that they have a defect in their electrical systems. Maybe we can use that to our advantage.";
                        Time.timeScale = 0F;
                        m_tutImage13.enabled = true;
                        break;
                    case 12:
                        m_TutorialTitleText.text="Phantom Ukko";
                        m_TutorialText.text = "I wouldn't belive it if I didn't see it with my own eyes. A ghost ship. I looks like it's transparent and hostile. I doubt that physical attacks are that effective.";
                        Time.timeScale = 0F;
                        m_tutImage14.enabled = true;
                        break;
                    case 13:
                        m_TutorialTitleText.text="Otsonian Droid";
                        m_TutorialText.text = "Oh darn! It's an Otsonian Battle Droid. Those things are among the most durable things in the galaxy and they hunt everything that doesn't fly under Otsonian alliance. I hope we have something that can penetrate that hull.";
                        Time.timeScale = 0F;
                        m_tutImage15.enabled = true;
                        break;
                    case 14:
                        m_TutorialTitleText.text="Grob Cube";
                        m_TutorialText.text = "Grob also known as the killer whale of space. It softens it's prey with some sort energy beam and then digests the prey whole. If we can alter it's radioactive frequencFy we might be able to make the creature's molecular structure to fold on it self.";
                        Time.timeScale = 0F;
                        m_tutImage16.enabled = true;
                        break;
                    case 15:
                        m_TutorialTitleText.text="Galactic Offender";
                        m_TutorialText.text = "It's one of the three Offenders. They are a group of space vigilantes that claim that they have been chosen to protect the galaxy by some mystical giant robots. Big blunt attacks might be effective against them.";
                        Time.timeScale = 0F;
                        m_tutImage17.enabled = true;
                        break;
                    default:
                        break;
                }
            }
            else if (PlayerPrefX.GetBool("Tutorial")==true   && m_tutType ==1)
            {
                m_tutorialPanel.SetActive(true);
                switch (situation)
                {
                    case 1:
                        m_TutorialTitleText.text="Sammon Tähti";
                        m_TutorialText.text = "Löysimme yhden Sammon Tähden. Enää kahdeksan osaa löydettävänä.";
                        m_tutImage5.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 2:
                        m_TutorialTitleText.text="Avaruusromua";
                        m_TutorialText.text = "Voimme käyttää tämän räjähtäneen aluksen osia korjataksemme omaa alustamme. Saamme korjattua aluksen runkoa 20% ja kilpiä 10%.";
                        m_tutImage11.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 3:
                        m_TutorialTitleText.text="Tehostettu tutka-alue";
                        m_TutorialText.text = "Tällä alueella vähemmän singnaalihäiriötä ja saamme paremman kuvan ympäröivistä vaaroista. Ne näkyvät kartalla vain hetken aikaa, joten pyri muistamaan nämä paikat.";
                        m_tutImage6.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 4:
                        m_TutorialTitleText.text="Satelliitti";
                        m_TutorialText.text = "Saimme liitettyä aluksemme galaktiseen satelliitti verkkoon ja voimme skannata osan paikallista aurinkokuntaa. Merkitse alue jonka haluat tutkia painamalla valitsemasi alueen keskelle.";
                        m_tutImage9.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 5:
                        m_TutorialTitleText.text="Avaruus kartta";
                        m_TutorialText.text = "Löysimme aarrekartan, mutta se tuhoutuu nopeasti käytön jälkeen, joten näemme Tähden sijainnin vain hetken kartalla. Vaikuttaisi että kartta etsii lähimmän Sammon osan.";
                        m_tutImage8.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 6:
                        m_TutorialTitleText.text="Hylätty Rahti Alus";
                        m_TutorialText.text = "Saimme kalastettua hylätyn rahdin ja täydensimme ammusvarastoamme.";
                        m_tutImage7.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 7:
                        m_TutorialTitleText.text="Asteroidi vyöhyke";
                        m_TutorialText.text = "Navigoimme vahingossa suoraan asteroidi myrskyyn ja törmäys vahingoitti runkoa sekä kilpiä.";
                        m_tutImage12.enabled = true;
                        Time.timeScale = 0F;
                        break;
                    case 8:
                        m_TutorialTitleText.text = "Liikkuminen";
                        m_TutorialText.text = "Alusta ympäröiviä nuolia käytetään liikkumiseen. Painamalla nuolta liikut yhden ruudun valitsemaasi suuntaan.";
                        m_tutImage1.enabled = true;
                        m_tutImage2.enabled = true;
                        m_tutImage3.enabled = true;
                        m_tutImage4.enabled = true;
                        break;
                    case 9:
                        m_TutorialTitleText.text="Tehtävän anto";
                        m_TutorialText.text = "Uskomme Tarunomaisen Sammon Tähtien olevan tässä aurinkokunnassa. Alue on vaarallinen ja resurssimme ovat rajalliset. Paikallinen sektori on tunnettu vaarallisista paikallisistaan, joten fiksu ja hidas alueen kartoittaminen on suotavaa.";
                        m_tutImage5.enabled = true;
                        m_tutnum = 9;
                        break;
                    case 10:
                        m_TutorialTitleText.text="Planeetat";
                        m_TutorialText.text = "Voimme tankata planeetoilla. Koska olemme vaarallisilla sektoreilla niin planeetoilla ei ole paljoa resursseja, joten voimme tankata vain kerran per planeetta. Planeetat joilla emme ole vielä vierailleet on merkitty vihreillä tankkeri symboleilla. Tankkaus korjaa 20% rungon pisteitä, 10% kilpi pisteitä ja täydentää yhden jokaista ammusmallia.";
                        m_tutImage10.enabled = true;
                        break;
                    case 11:
                        m_TutorialTitleText.text="Paikalliset Piraatit";
                        m_TutorialText.text = "Paikallinen roskasakki on saapunut ja he haluavat aarteemme. Ei mitään mitä emme pystyisi hoitamaan. Huhujen mukaan heidän aluksensa kestävät iskuja, mutta niihin tulee helposti sähkövikoja.";
                        Time.timeScale = 0F;
                        m_tutImage13.enabled = true;
                        break;
                    case 12:
                        m_TutorialTitleText.text="Häivähdys Ukko";
                        m_TutorialText.text = "En uskoisi ellen itse näkisi, kuin aavelaiva. Sen runko on läpinäkyvä ja se hohkaa energiaa. En usko että fyysiset hyökkäykset toimivat kovin hyvin sitä vastaan.";
                        Time.timeScale = 0F;
                        m_tutImage14.enabled = true;
                        break;
                    case 13:
                        m_TutorialTitleText.text="Otsonialainen Droidi";
                        m_TutorialText.text = "Voi Pas...! Otsonialainen Taistelu Droidi. Nuo alukset ovat Galaksin kestävävimpiä aluksia ja ne hyökkäävät kaikkien kimppuun, jotka eivät lennä Otsonian lipun alla. Toivottavasti meillä on jotain joka läpäisee tuon suojauksen";
                        Time.timeScale = 0F;
                        m_tutImage15.enabled = true;
                        break;
                    case 14:
                        m_TutorialTitleText.text="Grob Kuutio";
                        m_TutorialText.text = "Grob tunnetaan myös avaruuden tappajavalaana. Se pehmittää saalistaan ensin pommittamalla energialla jonka jälkeen se nielaisee saaliinsa kokonaisena. Jos saamme muutettua sen radioaktiivista taajuutta sen ruumiin molekyylien koostomus saattaa kaatua omasta massastaan.";
                        Time.timeScale = 0F;
                        m_tutImage16.enabled = true;
                        break;
                    case 15:
                        m_TutorialTitleText.text="Galaktinen Hulttio";
                        m_TutorialText.text = "Se on yksi kolmesta Hulttiosta. He ovat joukko lainsuojattomia jotka uskovat, että he suojelevat galaksia ampumalla kaikki.";
                        Time.timeScale = 0F;
                        m_tutImage17.enabled = true;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                return;
            }
        }
        public void DefeatText(int EnemyNum)
        {
            if (m_tutType == 0)
            {
                switch (EnemyNum)
                {
                    case 1://pirate
                        m_DefeatText.text = "Your ship and crew met their demise while fightin Pirates. By some weird twist of fate you survived, with the knowledge that better planning is the key to success.";
                        break;
                    case 2://ghost
                        m_DefeatText.text = "Your ship and crew were electrocuted by the Phantom. The Reaper wasn't ready to take you yet and so you will live with the knowledge that better planning is the key to success.";
                        break;
                    case 3://droid
                        m_DefeatText.text = "Your ship and crew were vaporized by the Otsonian Droid. By beaming your self to a nearby planet you survived, with the knowledge that better planning is the key to success.";
                        break;
                    case 4://cube
                        m_DefeatText.text = "Your ship and crew got eaten by the Grob. You seemed to give the creature indigestion so it spit you out. You survived with the knowledge that better planning is the key to success.";
                        break;
                    case 5://offender
                        m_DefeatText.text = "Your ship and crew were destroyed by the Offender. He saw you as a worthy rival and let you go in hopes of meeting you again. You returned with the knowledge that better planning is the key to success.";
                        break;
                    case 6://asteroid
                        m_DefeatText.text = "Your ship crached into asteroid fiels. By some weird twist of fate you survived, with the knowledge that better planning is the key to success.";
                        break;
                    case 7://lazer
                        m_DefeatText.text = "Your ship and crew met their demise because you overcharged lasers. By some weird twist of fate you survived, with the knowledge that better planning is the key to success.";
                        break;
                    default:
                        break;
                }
            }
            else if (m_tutType == 1)
            {
                switch (EnemyNum)
                {
                    case 1://pirate
                        m_DefeatText.text = "Aluksesi menehtyi taistelussa piraatteja vastaan.";
                        break;
                    case 2://ghost
                        m_DefeatText.text = "Aluksesi menehtyi taistelussa Kummitus Alusta vastaan.";
                        break;
                    case 3://droid
                        m_DefeatText.text = "Aluksesi menehtyi taistelussa Otsonialaista Droidia vastaan.";
                        break;
                    case 4://cube
                        m_DefeatText.text = "Aluksesi menehtyi taistelussa Grob Kuutiota vastaan.";
                        break;
                    case 5://offender
                        m_DefeatText.text = "Aluksesi menehtyi taistelussa Hulttiota vastaan.";
                        break;
                    case 6://asteroid
                        m_DefeatText.text = "Aluksesi menehtyi Asteroidi kolarissa.";
                        break;
                    case 7://lazer
                        m_DefeatText.text = "Aluksesi menehtyi Moottorien ylikuumenemseen.";
                        break;
                    default:
                        break;
                }
            }
        }

        public void GameOver(int enemy)
        {
            DefeatText(enemy);
            AudioScript.m_defeat = true;
            m_gameoverPanel.SetActive(true);
        }
        public void Victory()
        {
            AudioScript.m_victory = true;
            m_playerRef.GetComponent<PlayerReference>().Score();
            m_victoryPanel.SetActive(true);

        }
        public void PlayAgain()
        {
            SceneManager.LoadScene("MediumDifficulty");
        }
        public void MainMenu()
        {
            SceneManager.LoadScene("StartMenu");
        }
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
