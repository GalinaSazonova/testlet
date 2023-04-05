namespace TestletRandom
{
    public class Testlet
    {
        public string TestletId;
        private List<Item> Items;
        public Testlet(string testletId, List<Item> items)
        {
            CheckItemsInput(items);
            TestletId = testletId;
            Items = items;
        }
        /// <summary>
        /// Items private collection has 6 Operational and 4 Pretest Items.
        /// Randomize the order of these items as per the requirement (with TDD)
        /// </summary>
        /// <returns>ordered item list</returns>
        public List<Item> Randomize()
        {
            var random = new Random();

            var result = new List<Item>(TestletConsts.ItemCount);
            var pretestItems = Items.Where(item => item.ItemType == ItemTypeEnum.Pretest).ToList();

            for (var i = 0; i < TestletConsts.PretestStartCount; i++)
            {
                var index = random.Next(pretestItems.Count);
                result.Add(pretestItems[index]);
                pretestItems.RemoveAt(index);
            }

            var rest = Items.Where(item => item.ItemType == ItemTypeEnum.Operational).Union(pretestItems).ToList();
            for (var j = 0; j < TestletConsts.ItemCount - TestletConsts.PretestStartCount; j++)
            {
                var index = random.Next(rest.Count);
                result.Add(rest[index]);
                rest.RemoveAt(index);
            }

            return result;
        }

        private void CheckItemsInput(List<Item> items)
        {
            if (items == null || items.Count != TestletConsts.ItemCount)
            {
                throw new Exception("Wrong number of items");
            }

            if (items.Count(item => item.ItemType == ItemTypeEnum.Pretest) != TestletConsts.PretestTotalCount)
            {
                throw new Exception("Wrong pretest/operational ratio");
            }
        }
    }
}
