using TestletRandom;

namespace UnitTests
{
    public class TestletUnitTests
    {
        private List<Item> GetTestletItems()
        {
            var items = new List<Item>(TestletConsts.ItemCount);
            for (int i = 0; i < TestletConsts.PretestTotalCount; i++)
            {
                items.Add(new Item { ItemId = $"pretest{i}", ItemType = ItemTypeEnum.Pretest });
            }

            for (int j = 0; j < TestletConsts.OperationalTotalCount; j++)
            {
                items.Add(new Item { ItemId = $"operational{j}", ItemType = ItemTypeEnum.Operational });
            }

            return items;
        }

        [Fact]
        public void Create_Testlet_With_Empty_List_Returns_Exception()
        {
            //Arrange
            var items = new List<Item>();
            var id = "testlet";

            //Act
            var exception = Record.Exception(() => new Testlet(id, items));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Wrong number of items", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Less_Items_Count_Returns_Exception()
        {
            //Arrange
            var items = new List<Item>();
            var id = "testlet";

            for (int i = 0; i < TestletConsts.ItemCount - 1; i++)
            {
                items.Add(new Item { ItemId = $"pretest{i}", ItemType = ItemTypeEnum.Pretest });
            }

            //Act
            var exception = Record.Exception(() => new Testlet(id, items));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Wrong number of items", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Exceed_Items_Count_Returns_Exception()
        {
            //Arrange
            var items = new List<Item>();
            var id = "testlet";

            for (int i = 0; i < TestletConsts.ItemCount + 1; i++)
            {
                items.Add(new Item { ItemId = $"pretest{i}", ItemType = ItemTypeEnum.Pretest });
            }

            //Act
            var exception = Record.Exception(() => new Testlet(id, items));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Wrong number of items", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Wrong_Pretest_Perational_Ratio_Returns_Exception()
        {
            //Arrange
            var items = GetTestletItems();
            var id = "testlet";

            foreach(var item in items)
            {
                item.ItemType = ItemTypeEnum.Operational;
            }

            //Act
            var exception = Record.Exception(() => new Testlet(id, items));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("Wrong pretest/operational ratio", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Null_List_Returns_Exception()
        {
            //Arrange
            var id = "testlet";

            //Act
            var exception = Record.Exception(() => new Testlet(id, null));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains("items expected", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Correct_Conditions_Returns_No_Exception()
        {
            //Arrange
            var items = GetTestletItems();
            var id = "testlet";

            //Act
            var exception = Record.Exception(() => new Testlet(id, items));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void TestletRandomizer_Returns_Correct_ItemCount()
        {
            //Arrange
            var testlet = new Testlet("id", GetTestletItems());

            //Act
            var result = testlet.Randomize();

            // Assert
            Assert.Equal(TestletConsts.ItemCount, result.Count);
        }

        [Fact]
        public void TestletRandomizer_Returns_PretestStartCount_Pretests_At_Begging()
        {
            //Arrange
            var testlet = new Testlet("id", GetTestletItems());

            //Act
            var result = testlet.Randomize();
            var resultPrestestStartCount = result.Take(TestletConsts.PretestStartCount).Count(item => item.ItemType == ItemTypeEnum.Pretest);

            // Assert
            Assert.Equal(TestletConsts.PretestStartCount, resultPrestestStartCount);
        }

        [Fact]
        public void TestletRandomizer_Returns_OperationalCount_and_PretestCount_Items()
        {
            //Arrange
            var testlet = new Testlet("id", GetTestletItems());

            //Act
            var result = testlet.Randomize();

            var operationalCount = 0;
            var pretestCount = 0;

            foreach (var item in result)
            {
                if (item.ItemType == ItemTypeEnum.Pretest)
                {
                    pretestCount++;
                    continue;
                }
                if (item.ItemType == ItemTypeEnum.Operational)
                {
                    operationalCount++;
                }
            }

            // Assert
            Assert.Equal(TestletConsts.OperationalTotalCount, operationalCount);
            Assert.Equal(TestletConsts.PretestTotalCount, pretestCount);
        }

        [Fact]
        public void TestletRandomizer_Do_Not_Affect_Input()
        {
            //Arrange
            var items = GetTestletItems();
            var testlet = new Testlet("id", items);
            var initialItems = new List<Item>(items);

            //Act
            _ = testlet.Randomize();

            // Assert
            Assert.Equal(initialItems, items);
        }

        [Fact]
        public void TestletRandomizer_Returns_Only_Items_From_Input()
        {
            //Arrange
            var items = GetTestletItems();
            var testlet = new Testlet("id", items);

            //Act
            var result = testlet.Randomize();

            // Assert
            foreach (var item in result)
            {
               Assert.Contains(item, items);
            }
        }

        [Fact]
        public void TestletRandomizer_Returns_Unique_Items()
        {
            //Arrange
            var items = GetTestletItems();
            var testlet = new Testlet("id", items);

            //Act
            var result = testlet.Randomize();
            var uniqueItemsCount = result.Select(item => item.ItemId).Distinct().Count();

            //Assert
            Assert.Equal(TestletConsts.ItemCount, uniqueItemsCount);
        }
    }
}