namespace TestletRandom
{
    public class Testlet
    {
        public string TestletId { get; }
        private readonly List<Item> _items;

        public Testlet(string testletId, List<Item> items)
        {
            CheckItemsInput(items);
            TestletId = testletId;
            _items = items;
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
            var pretestItems = _items.Where(item => item.ItemType == ItemTypeEnum.Pretest).ToList();

            for (var i = 0; i < TestletConsts.PretestStartCount; i++)
            {
                var index = random.Next(pretestItems.Count);
                result.Add(pretestItems[index]);
                pretestItems.RemoveAt(index);
            }

            result.AddRange(_items.Where(item => item.ItemType == ItemTypeEnum.Operational).Union(pretestItems));
            for (var j = TestletConsts.ItemCount - 1; j >= TestletConsts.PretestStartCount; j--)
            {
                var index = random.Next(TestletConsts.PretestStartCount, j + 1);
                var temp = result[index];
                result[index] = result[j];
                result[j] = temp;
            }

            return result;
        }

        private void CheckItemsInput(List<Item> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException($"{TestletConsts.ItemCount} items expected.");
            }

            if (items.Count != TestletConsts.ItemCount)
            {
                throw new Exception($"Wrong number of items. {TestletConsts.ItemCount} expected, {items.Count} received");
            }

            if (items.Count(item => item.ItemType == ItemTypeEnum.Pretest) != TestletConsts.PretestTotalCount)
            {
                throw new Exception("Wrong pretest/operational ratio");
            }
        }
    }
}
