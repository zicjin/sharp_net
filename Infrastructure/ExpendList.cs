using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharp_net {
    public class ExpendList<T> {

        public void RemoveMultiple(IEnumerable<T> itemsToRemove) {
            var removingItems = new Dictionary<T, int>();
            T[] _items = itemsToRemove.ToArray();
            foreach (var item in itemsToRemove) {
                if (removingItems.ContainsKey(item)) {
                    removingItems[item]++;
                } else {
                    removingItems[item] = 1;
                }
            }

            var setIndex = 0;

            for (var getIndex = 0; getIndex < _items.Count(); getIndex++) {
                var current = _items[getIndex];
                if (removingItems.ContainsKey(current)) {
                    removingItems[current]--;
                    if (removingItems[current] == 0) {
                        removingItems.Remove(current);
                    }
                    continue;
                }
                _items[setIndex++] = _items[getIndex];
            }
            itemsToRemove = _items;
        }
    }
}
