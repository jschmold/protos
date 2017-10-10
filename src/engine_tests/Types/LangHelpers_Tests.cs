using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Engine.LangHelpers;
namespace EngineTests.Types {
    [TestClass]
    [TestCategory("LangHelpers")]
    public class LangHelpers_Tests {
        [TestMethod]
        public void DoOrThrow_DoesAction() {
            bool works = false;
            DoOrThrow(() => works = true, new Exception("This should never happen"));
            Assert.IsTrue(works, "Did not call Action instead of Exception");
        }
        [TestMethod]
        public void DoOrThrow_ThrowsException() {
            Assert.ThrowsException<Exception>(() => DoOrThrow(null, new Exception("Yee")));
        }
        [TestMethod]
        public void PerformIfBool_PerformsIfTrue() {
            bool works = true;
            Perform(works, () => works = false);
            Assert.IsFalse(works, "Did not modify works, must not have called function");
        }
        [TestMethod]
        public void PerformIfBool_DoesNothingIfFalse() {
            bool works = false;
            Perform(works, () => works = true);
            Assert.IsFalse(works, "Modified works. Should not have.");
        }
        [TestMethod]
        public void PerformIfDelegate_PerformsIfTrue() {
            bool works = false;
            Perform(() => true, () => works = true);
            Assert.IsTrue(works, "Did not modify works, and should have");
        }
        [TestMethod]
        public void PerformIfDelegate_DoesNothingIfFalse() {
            bool works = true;
            Perform(() => false, () => works = false);
            Assert.IsTrue(works, "Modified works and should not have");
        }
        private static bool simplePerformTest = true;
        [TestMethod]
        public void PerformIf_ExpressionBodyIsSimple() => Perform(true, () => {
            simplePerformTest = false;
            Assert.IsFalse(simplePerformTest);
        });
        static int composecounter = 1;
        [TestMethod]
        public void Compose_RunsAll() => Compose(
            () => composecounter += 1,
            () => composecounter *= 2,
            () => Assert.IsTrue(composecounter == 4),
            () => composecounter = 1
            );

        private int PipeAddOne(int x) => x + 1;
        private int PipeMultiTwo(int x) => x * 2;
        private int PipeOp(int num) => Pipe(num, PipeAddOne, PipeMultiTwo);

        [TestMethod]
        public void Pipe_ModifiesValueEachTime() {
            int x = 1;
            x = PipeOp(x);
            Assert.IsTrue(x == 4, $"Did not perform pipe operation properly. Expected 4, actually {x}");
        }
    }
}
