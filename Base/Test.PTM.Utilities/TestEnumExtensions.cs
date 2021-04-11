using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PTM.Utilities;

namespace Test.PTM.Utilities
{
    [TestClass]
    public class TestEnumExtensions
    {
        /// <summary>
        /// Testy enum z opisem
        /// </summary>
        private enum WithDescription
        {
            [EnumDescription("test")]
            One,
            [EnumDescription("test")]
            Two,
            [EnumDescription("test")]
            Three
        }

        /// <summary>
        /// Testy enum bez opisu
        /// </summary>
        private enum WithoutDescription
        {
            One,
            Two,
            Three
        }

        /// <summary>
        /// Sprawdza czy zwracona jest prawid�owa warto�� dla enum�w posiadaj�cych atrybut Description
        /// </summary>
        [TestMethod]
        public void GetDescription_ReturnString_IsCorrect()
        {
            //Arrange
            var DescribedEnums = Enum.GetValues(typeof(WithDescription));

            //Act, Assert
            foreach (Enum Enum in DescribedEnums)
            {
                Assert.AreEqual(Enum.GetDescription(), "test");
            }
        }

        /// <summary>
        /// Sprawdza czy warto�� nie jest zwracana dla enum�w nie posiadaj�cych atrybutu Description
        /// </summary>
        [TestMethod]
        public void GetDescription_ReturnString_IsEmpty()
        {
            //Arrange
            var NormalEnums = Enum.GetValues(typeof(WithoutDescription));

            //Act, Assert
            foreach (Enum Enum in NormalEnums)
            {
                Assert.AreEqual(Enum.GetDescription(), string.Empty);
            }
        }
    }
}
