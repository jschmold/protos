using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Engine.Types;
using Engine.Exceptions;
using System.Reflection;
using Engine.Entities;
using System.Timers;
using Engine.Constructables;
using Engine.Helpers;
using Engine.Interfaces;

namespace EngineTests.Types {
    [TestClass]
    [TestCategory("ResearchBay")]
    public class ResearchBay_Tests {
        static Resource Scrap = new Resource {
            Identifier = 0x0000001,
            Name = "Scrap",
            Volume = 10,
            Mass = 1
        };
        static Knowledge Science = new Knowledge {
            WorkerCost = 5,
            KnowledgeRequirements = null,
            ResourceRequirements = null,
            StationCost = 2,
            Unlocks = new Skill {
                Description = "Science, bitch",
                Identifier = 0x0000001,
                Name = "Science"
            }
        };
        static Knowledge Math = new Knowledge {
            WorkerCost = 5,
            KnowledgeRequirements = null,
            ResourceRequirements = null,
            StationCost = 2,
            Unlocks = new Skill {
                Description = "Math, bitch",
                Identifier = 0x0000002,
                Name = "Math"
            }
        };

        static Knowledge Physics = new Knowledge {
            WorkerCost = 5,
            KnowledgeRequirements = new List<Knowledge> {
                Math, Science
            },
            ResourceRequirements = null,
            StationCost = 1,
            Unlocks = new Skill {
                Description = "Physics",
                Identifier = 0x0000003,
                Name = "Physics"
            }
        };
        static Knowledge AdvancedPhysics = new Knowledge {
            WorkerCost = 5,
            KnowledgeRequirements = new List<Knowledge> {
                Math, Science, Physics
            },
            ResourceRequirements = new List<Quantified<Resource>> {
                new Quantified<Resource>(Scrap, 5)
            },
            StationCost = 2,
            Unlocks = new Skill {
                Description = "Physics",
                Identifier = 0x0000003,
                Name = "Physics"
            }
        };
        private List<IPowerSource> PowerGrid(uint size) {
            PowerCellCluster cluster = new PowerCellCluster(1);
            cluster.Add(size, size, 0, 0);
            return new List<IPowerSource> { cluster };
        }
        public ResearchBay GetBay => new ResearchBay(100, 10, new List<Knowledge> { Science, Math }, 100, PowerGrid(100), 1000);
        private ResearchBay ScienceBay => new ResearchBay(4, 5, new List<Knowledge> {
                Science
            }, 100, PowerGrid(1000), 1000);
        private ResearchBay MathBay => new ResearchBay(4, 5, new List<Knowledge> {
                Math
            }, 100, PowerGrid(1000), 1000);
        private ResearchBay PhysicsBay => new ResearchBay(4, 5, new List<Knowledge> {
                Physics
            }, 100, PowerGrid(1000), 1000);
        private ResearchBay AdvPhysicsBay => new ResearchBay(4, 5, new List<Knowledge> {
            Physics, AdvancedPhysics
        }, 100, PowerGrid(1000), 1000);
        private Citizen MathWorker => new Citizen {
            Energy = new Bank {
                Maximum = 100,
                Quantity = 100
            },
            Skills = new List<Skill> {
                Math.Unlocks
            }
        };
        private Citizen ScienceWorker => new Citizen {
            Energy = new Bank {
                Maximum = 100,
                Quantity = 100
            },
            Skills = new List<Skill> {
                Science.Unlocks
            }
        };
        private Citizen PhysicsWorker = new Citizen {
            Energy = new Bank {
                Maximum = 10000,
                Quantity = 10000
            },
            Skills = new List<Skill> {
                Science.Unlocks,
                Math.Unlocks
            }
        };
        private QuantifiedBank<Knowledge> ScienceProg = new QuantifiedBank<Knowledge> {
            Currency = Science,
            Maximum = 100,
            Quantity = 0
        };
        private QuantifiedBank<Knowledge> PhysicsProg = new QuantifiedBank<Knowledge> {
            Currency = Physics,
            Maximum = 100,
            Quantity = 0
        };
        private PropertyInfo Active => typeof(ResearchBay).GetProperty("Active");
        private PropertyInfo Recovery => typeof(ResearchBay).GetProperty("Recovering", BindingFlags.Instance | BindingFlags.NonPublic);

        [TestMethod]
        public void AddResearcher_AddsResearcher() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);
            bay.AddResearcher(new Citizen( ));
            Assert.IsTrue(bay.Researchers.Count == 1, $"Expected 1, actual {bay.Researchers.Count}");
        }

        [TestMethod]
        public void AddResearcher_ListensToLimit() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);
            for (int i = 0 ; i < 3 ; i++) {
                bay.AddResearcher(new Citizen( ));
            }
            Assert.ThrowsException<LimitMetException>(() => bay.AddResearcher(new Citizen( )), "Expected to error on 4th addition.");
        }

        [TestMethod]
        public void ConvertReseacherEnergy_AddsToActive() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);
            var cit = new Citizen {
                Energy = new Bank {
                    Maximum = 1000,
                    Quantity = 1000
                }
            };

            var activeProperty = typeof(ResearchBay)
                .GetProperty("Active");

            activeProperty.SetValue(bay, ScienceProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            bay.ConvertResearcherEnergy(cit);
            Assert.IsTrue(bay.Active.Quantity == 5, $"Expected active progress to be 5, actual {bay.Active.Quantity}");
        }

        [TestMethod]
        public void ConvertResearchEnergy_RemovesFromResearcher() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);
            var cit = new Citizen {
                Energy = new Bank {
                    Maximum = 1000,
                    Quantity = 1000
                }
            };

            var activeProperty = typeof(ResearchBay)
                .GetProperty("Active");


            activeProperty.SetValue(bay, ScienceProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            bay.ConvertResearcherEnergy(cit);
            Assert.IsTrue(cit.Energy == 995, $"Expected citizen energy to be 955, actual {cit.Energy.Quantity}");
        }

        [TestMethod]
        public void Research_ThrowsIfActiveNotNull() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);

            Active.SetValue(bay, ScienceProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            Assert.ThrowsException<InvalidOperationException>(() => bay.Research(Science));
        }

        [TestMethod]
        public void Research_SetsActive() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);
            bay.Research(Science);
            Assert.IsNotNull(bay.Active, "Expected active to be set.");
        }

        [TestMethod]
        public void Research_RefusesUnsupported() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);

            Assert.ThrowsException<UnsupportedException>(() => bay.Research(Math), "Expected researching to fail");
        }

        [TestMethod]
        public void Research_ExpendsResources() {
            var bay = AdvPhysicsBay;
            bay.Resources.Maximum = 100000;
            var wk = PhysicsWorker;
            bay.Resources.Add(Scrap, 100);
            bay.Research(AdvancedPhysics);
            Assert.IsTrue(bay.Resources[Scrap] == 95, $"Expected scrap to be 95, actually: {bay.Resources[Scrap]}");
        }

        [TestMethod]
        public void Research_RefusesOnInsufficientMaterials() {
            var bay = AdvPhysicsBay;
            var wk = PhysicsWorker;
            bay.Resources.Add(Scrap, 4);
            Assert.ThrowsException<LackingResourceException>(() => bay.Research(AdvancedPhysics), "Should have errored, as only 4 of 5 scrap were in the resources");
        }

        [TestMethod]
        public void Cancel_ClearsActive() {
            ResearchBay bay = new ResearchBay(0, 3, new List<Knowledge> {
                Science
            }, 0, PowerGrid(1000), 1000);
            var activeProperty = typeof(ResearchBay).GetProperty("Active");

            activeProperty.SetValue(bay, ScienceProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            bay.Cancel( );
            Assert.IsNull(bay.Active);
        }
        
        [TestMethod]
        public void IsQualified_FalseOnUnqualified() {
            var bay = PhysicsBay;
            Active.SetValue(bay, PhysicsProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            Assert.IsFalse(bay.IsQualified(MathWorker), "Math worker is not qualified, but says it is");
        }

        [TestMethod]
        public void IsQualified_TrueOnQualified() {
            var bay = PhysicsBay;
            Active.SetValue(bay, PhysicsProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            Assert.IsTrue(bay.IsQualified(PhysicsWorker), "Physics worker is qualified, but says they are not");
        }

        [TestMethod]
        public void ExpendEnergyFromActive_DoesProperAmount() {
            var bay = PhysicsBay;
            Active.SetValue(bay, PhysicsProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            bay.ExpendEnergyFromActive( );
            var avail = IPowerableUtils.PowerAvailable(bay.EnergySources);
            Assert.IsTrue(avail == 999, $"Expected energy to drop to 999, actually {avail}");
        }

        [TestMethod]
        public void FinalizeResearch_AddsToRepo() {
            var bay = PhysicsBay;
            Active.SetValue(bay, PhysicsProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            bay.FinalizeResearch( );
            Assert.IsTrue(bay.IsResearched(Physics), "Expected finalizing to add to repo.");
        }

        [TestMethod]
        public void FinalizeResearch_ClearsActive() {
            var bay = PhysicsBay;
            Active.SetValue(bay, PhysicsProg, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            bay.FinalizeResearch( );
            Assert.IsNull(bay.Active, "Should have cleared active");
        }

        [TestMethod]
        public void FinalizeResearch_GivesSkillToAllQualifiedResearchersInBay() {
            var bay = PhysicsBay;
            var QualWorkers = new List<Citizen> {
                PhysicsWorker, PhysicsWorker
            };
            var UnqualWorkers = new List<Citizen> {
                MathWorker, MathWorker
            };
            QualWorkers.ForEach(wk => bay.AddResearcher(wk));
            UnqualWorkers.ForEach(wk => bay.AddResearcher(wk));
            bay.Research(Physics);
            bay.Active.Quantity = bay.Active.Maximum;
            bay.FinalizeResearch( );
            foreach (var wk in QualWorkers) {
                Assert.IsTrue(wk.Skills.Contains(Physics.Unlocks));
            };  
            foreach (var wk in UnqualWorkers) {
                Assert.IsFalse(wk.Skills.Contains(Physics.Unlocks));
            }
        }

        [TestMethod]
        public void ProcessResearchers_RestsTiredResearchers() {
            var bay = PhysicsBay;
            var tired = new List<Citizen> {
                PhysicsWorker, PhysicsWorker, PhysicsWorker
            };
            tired.ForEach(wk => {
                wk.Energy.Quantity = 5;
                wk.Energy.Maximum = 100;
                bay.AddResearcher(wk);
            });
            bay.AddResearcher(PhysicsWorker);
            bay.ProcessResearchers( );
            var rec = Recovery.GetValue(bay) as List<Citizen>;
            Assert.IsTrue(tired.TrueForAll(wk => rec.Contains(wk)), "Did not contain all tired workers");
        }

        [TestMethod]
        public void ProcessResearchers_PutsRecoveredResearchersToWork() {
            var bay = PhysicsBay;
            var tired = new List<Citizen> {
                PhysicsWorker, PhysicsWorker, PhysicsWorker
            };
            for (int i = 0 ; i < 3 ; i++) {
                bay.AddResearcher(tired[i]);
            }
            Recovery.SetValue(bay, tired);
            Assert.IsTrue((Recovery.GetValue(bay) as List<Citizen>).Count == 3, "Did not put workers in properly");
            bay.ProcessResearchers( );
            Assert.IsTrue((Recovery.GetValue(bay) as List<Citizen>).Count == 0, "Did not remove workers from recovery");
        }

        [TestMethod]
        public void Think_ProducesResults() {
            var bay = new ResearchBay(4, 5, new List<Knowledge> {
                Physics
            }, 100, PowerGrid(10000000), 1000000);
            bay.AddResearcher(PhysicsWorker);
            bay.AddResearcher(PhysicsWorker);
            bay.AddResearcher(PhysicsWorker);
            bay.AddResearcher(PhysicsWorker);
            bay.AddResearcher(PhysicsWorker);
            bay.Research(Physics);

            Func<bool> AllResearchersHaveEnergy = () => bay.Researchers.TrueForAll(wk => wk.Energy.Quantity > 0);
            while (!bay.IsResearched(Physics)) {
                if (IPowerableUtils.PowerAvailable(bay.EnergySources) == 0) {
                    Assert.Fail("Test is faulty. Not enough energy in station to complete.");
                    return;
                }
                if (!AllResearchersHaveEnergy( )) {
                    Assert.Fail("Test is faulty. Not enough energy in workers to complete.");
                    return;
                }
                
                bay.Think( );
            }
            bay.Researchers.ForEach(wk => Assert.IsTrue(wk.Skills.Contains(Physics.Unlocks), "Did not give the physics skill to the researchers"));
            Assert.IsTrue(bay.IsResearched(Physics), "Did not complete research, but finished anyways");

        }
    }
}
