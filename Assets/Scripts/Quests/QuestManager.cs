using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{  

    static Dictionary<QuestName, Quest> quests;

    void Awake()
    {
        QuestManager.quests = new Dictionary<QuestName, Quest>();
        _instantiate_quests();
    }

    void Start()
    {
        //Progress quest if quest phase == 0
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            Quest quest = entry.Value;
            if (quest.currentPhase == 0)
            {
                EventManager.instance.QuestProgressed(quest.name, 0);
            }
        }
    }

    public static void ProgressQuest( QuestName quest )
    {
        ProgressQuestToPhase(quest, QuestManager.quests[quest].currentPhase + 1);
    }

    public static void ProgressQuestToPhase( QuestName quest, int phase )
    {
        print("Quest: " + quest + "Updated to phase: " + phase);
        QuestManager.quests[quest].currentPhase = phase;
        EventManager.instance.QuestProgressed( quest, quests[quest].currentPhase );
        if ( QuestManager.quests[quest].currentPhase >= QuestManager.quests[quest].totalPhases )
            CompletedQuest(quest);
    }

    public static void CompletedQuest( QuestName completedQuest )
    {
        foreach(KeyValuePair<QuestName, Quest> entry in quests)
        {
            QuestName currName = entry.Key;
            Quest curr = entry.Value;
            if ( curr.currentPhase == -1 && curr.prerequisites.Contains(completedQuest) )
            {
                curr.prerequisites.Remove(completedQuest);
                curr.prereqsCompleted++;

                if ( curr.prerequisites.Count == 0 || curr.prereqsCompleted >= curr.numPrereqsRequired )
                    ProgressQuest(currName);
            }
        }
    }

    public static int GetQuestPhase( QuestName quest )
    {
        return QuestManager.quests[quest].currentPhase;
    }

    public static Dictionary<QuestName, Quest> GetQuests()
    {
        return quests;
    }

    public static List<Quest> GetAvailableQuests()
    {
        List<Quest> availableQuests = new List<Quest>();
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            Quest curr = entry.Value;
            if (curr.currentPhase >= 0 && curr.currentPhase < curr.totalPhases)
            {
                availableQuests.Add(curr);
            }
        }

        return availableQuests;
    }

    private void _instantiate_quests()
    {
        // All quests have to be added to QuestName and then added here

        // Add a none quest so that random background characters can advance none 
        QuestManager.quests.Add( 
            QuestName.None, 
            new Quest( 
                QuestName.None,
                new List<QuestName>{},
                new List<( string hint, string image )> {}
            )
        );

        QuestManager.quests.Add( 
            QuestName.PerformDiagnostics, 
            new Quest( 
                QuestName.PerformDiagnostics,
                new List<QuestName>{},
                new List<( string hint, string image )> {
                    ( "Automatic", null ),
                    ( "Turn on the first portal by hitting the button", "QuestImages/FirstButton" ),
                    ( "Go through the newly active portal and collect your weapon", "QuestImages/SwordInStone" ),
                    ( "Return through the portal and destroy the box containing your tools", "QuestImages/FirstBox" ),
                    ( "Use your tools to fix the generator" , "QuestImages/BrokenGenerator" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.GettingALayOfTheLand, 
            new Quest( 
                QuestName.GettingALayOfTheLand, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Automatic", null ),
                    ( "Talk to the Bank Manager at Morgan's Panley", "QuestImages/Manager" ),
                    ( "Talk to the Shogun of Bakufu", "QuestImages/Shogun" ),
                    ( "Talk to the Chief of Fimbultoft", "QuestImages/VikingChief" ),
                })
        );

        QuestManager.quests.Add( 
            QuestName.ForGlory, 
            new Quest( 
                QuestName.ForGlory, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Shogun of Bakufu", "QuestImages/Shogun" ),
                    ( "Follow the Shogun", "QuestImages/Shogun2" ),
                    ( "Step into the arena", "QuestImages/Arena" ),
                    ( "Prepare to fight", "Images/test" ),
                    ( "Defeat Helga", "Images/test" ),
                    ( "Defeat Musashi The Machine","Images/test" ),
                    ( "Defeat Stacy From Accounting", "Images/test" ),
                    ( "Talk to the shogun (automatic)", "QuestImages/Shogun" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.WhateverFloatsYourBoat, 
            new Quest( 
                QuestName.WhateverFloatsYourBoat, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to Viking Chief", "QuestImages/VikingChief" ),
                    ( "Talk to Sven at the Docks", "QuestImages/Sven" ),
                    ( "Talk to Ormen at the Lumberyard", "QuestImages/Ormen" ),
                    ( "Take a Stack of Planks back to Sven","QuestImages/WoodPlanks" ),
                    ( "Listen to Sven", "QuestImages/Sven" ),
                    ( "Find Sigrid in her home", "QuestImages/Sigrid" ),
                    ( "Take the sail back to Sven", "QuestImages/Sigrid" ),
                    ( "Listen to Sven", "QuestImages/Sven" ),
                    ( "Talk to the Viking Chief", "QuestImages/VikingChief" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.AnIdiotsLuggage, 
            new Quest( 
                QuestName.AnIdiotsLuggage, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "QuestImages/Manager" ),
                    ( "Speak with Amanda the Teller", "QuestImages/Amanda" ),
                    ( "Speak with Damien in Security", "QuestImages/Damien" ),
                    ( "Speak with Olivia, the manager's assistant", "QuestImages/Olivia" ),
                    ( "Guess the Manager's Password", "QuestImages/VaultUnlock" ),
                    ( "Speak to the manager", "QuestImages/Manager" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.KarlTheJarl,
            new Quest(
                QuestName.KarlTheJarl,
                new List<QuestName>{ QuestName.WhateverFloatsYourBoat, QuestName.AnIdiotsLuggage },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager, Eugene Pants", "QuestImages/Manager" ),
                    ( "Go to Fimbultoft; Find Carl (maybe ask villager to tell you where he is)", "QuestImages/Carl" ),
                    ( "Find Carl's home-made shield in the Fimbulwald (the forest)", "QuestImages/Shield" ),
                    ( "Return Carl's Shield to him", "QuestImages/Carl" ),
                    ( "Return to the bank manager", "QuestImages/Manager" )
                }) 
        );

        // QuestManager.quests.Add( 
        //     QuestName.GoldenHandcuffs, 
        //     new Quest( 
        //         QuestName.GoldenHandcuffs, 
        //         new List<QuestName>{ QuestName.AnIdiotsLuggage },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         }) 
        // );

        // QuestManager.quests.Add( 
        //     QuestName.ButThisIsMyBirthCertificate, 
        //     new Quest( 
        //         QuestName.ButThisIsMyBirthCertificate, 
        //         new List<QuestName>{ QuestName.ForGlory },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         }) 
        // );
        
        // QuestManager.quests.Add( 
        //     QuestName.InternLunchHour, 
        //     new Quest( 
        //         QuestName.InternLunchHour, 
        //         new List<QuestName>{ QuestName.AnIdiotsLuggage },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         }) 
        // );

        // QuestManager.quests.Add( 
        //     QuestName.CalculatorCatastrophe, 
        //     new Quest( 
        //         QuestName.CalculatorCatastrophe, 
        //         new List<QuestName>{ QuestName.AnIdiotsLuggage, QuestName.ForGlory },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         }) 
        // );

        // QuestManager.quests.Add( 
        //     QuestName.ThisIsPureSnow, 
        //     new Quest( 
        //         QuestName.ThisIsPureSnow, 
        //         new List<QuestName>{ QuestName.AnIdiotsLuggage, QuestName.CarlTheJarl },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         }) 
        // );

        // QuestManager.quests.Add( 
        //     QuestName.ThingsAreGettingSerious, 
        //     new Quest( 
        //         QuestName.ThingsAreGettingSerious, 
        //         new List<QuestName>{ QuestName.GoldenHandcuffs, QuestName.ButThisIsMyBirthCertificate, QuestName.InternLunchHour, QuestName.CalculatorCatastrophe, QuestName.ThisIsPureSnow },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         },
        //         3 )
        // );

        // QuestManager.quests.Add( 
        //     QuestName.TheGreatWolf, 
        //     new Quest( 
        //         QuestName.TheGreatWolf, 
        //         new List<QuestName>{ QuestName.ThingsAreGettingSerious },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         })
        // );
        
        // QuestManager.quests.Add( 
        //     QuestName.BjornAndMoose, 
        //     new Quest( 
        //         QuestName.BjornAndMoose, 
        //         new List<QuestName>{ QuestName.ThingsAreGettingSerious },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         })
        // );

        // QuestManager.quests.Add( 
        //     QuestName.ForbiddenLovers, 
        //     new Quest( 
        //         QuestName.ForbiddenLovers, 
        //         new List<QuestName>{ QuestName.ThingsAreGettingSerious },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         })
        // );

        // QuestManager.quests.Add( 
        //     QuestName.ForTheUnion, 
        //     new Quest( 
        //         QuestName.ForTheUnion, 
        //         new List<QuestName>{ QuestName.ThingsAreGettingSerious },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         })
        // );

        // QuestManager.quests.Add( 
        //     QuestName.TheEnd, 
        //     new Quest( 
        //         QuestName.TheEnd, 
        //         new List<QuestName>{ QuestName.TheGreatWolf, QuestName.BjornAndMoose, QuestName.ForbiddenLovers, QuestName.ForTheUnion },
        //         new List<( string hint, string image )> {
        //             ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" )
        //         },
        //         3 )
        // );
    
    
    }
}
