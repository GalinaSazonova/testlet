using TestletRandom;
using Xunit.Abstractions;

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
        public void Create_Testlet_With_Wrong_Items_Count_Returns_Exception()
        {
            var items = new List<Item>();
            var id = "testlet";

            var exception = Record.Exception(() => new Testlet(id, items));

            Assert.NotNull(exception);
            Assert.Contains("Wrong number of items", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Wrong_Pretest_Perational_Ratio_Returns_Exception()
        {
            var items = GetTestletItems();
            var id = "testlet";

            foreach(var item in items)
            {
                item.ItemType = ItemTypeEnum.Operational;
            }

            var exception = Record.Exception(() => new Testlet(id, items));

            Assert.NotNull(exception);
            Assert.Contains("Wrong pretest/operational ratio", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Null_List_Returns_Exception()
        {
            var id = "testlet";

            var exception = Record.Exception(() => new Testlet(id, null));

            Assert.NotNull(exception);
            Assert.Contains("Wrong number of items", exception.Message);
        }

        [Fact]
        public void Create_Testlet_With_Correct_Conditions_Returns_No_Exception()
        {
            var items = GetTestletItems();
            var id = "testlet";

            var exception = Record.Exception(() => new Testlet(id, items));

            Assert.Null(exception);
        }

        [Fact]
        public void TestletRandomizer_Returns_Correct_ItemCount()
        {
            var testlet = new Testlet("id", GetTestletItems());

            var result = testlet.Randomize();

            Assert.Equal(TestletConsts.ItemCount, result.Count);
        }

        [Fact]
        public void TestletRandomizer_Returns_PretestStartCount_Pretests_At_Begging()
        {
            var testlet = new Testlet("id", GetTestletItems());

            var result = testlet.Randomize();

            Assert.Equal(TestletConsts.PretestStartCount, result.Take(TestletConsts.PretestStartCount).Count(item => item.ItemType == ItemTypeEnum.Pretest));
        }

        [Fact]
        public void TestletRandomizer_Returns_OperationalCount_and_PretestCount_Items()
        {
            var testlet = new Testlet("id", GetTestletItems());

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

            Assert.Equal(TestletConsts.OperationalTotalCount, operationalCount);
            Assert.Equal(TestletConsts.PretestTotalCount, pretestCount);
        }

        [Fact]
        public void TestletRandomizer_Do_Not_Affect_Input()
        {
            var items = GetTestletItems();
            var testlet = new Testlet("id", items);
            var initialItems = new List<Item>(items);

            _ = testlet.Randomize();

            Assert.Equal(initialItems, items);

        }

        [Fact]
        public void TestletRandomizer_Returns_Only_Items_From_Input()
        {
            var items = GetTestletItems();
            var testlet = new Testlet("id", items);

            var result = testlet.Randomize();

            foreach(var item in result)
            {
               Assert.Contains(item, items);
            }
        }
    }
}