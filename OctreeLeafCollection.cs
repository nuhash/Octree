using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;


class OctreeLeafCollection : Collection<OctreeLeaf>
{
    bool sorted = false;

    public bool Sorted
    {
        get { return sorted; }
        private set { sorted = value; }
    }
    int[] levels = new int[17]{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
    protected override void InsertItem(int index, OctreeLeaf item)
    {

        levels[item.Level] += 1;
        sorted = false;
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, OctreeLeaf item)
    {
        levels[item.Level] += 1;
        levels[this[index].Level] -= 1;
        sorted = false;
        base.SetItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        levels[this[index].Level] -= 1;
        base.RemoveItem(index);
        
    }

    private void Sort()
    {
        ((List<OctreeLeaf>)this.Items).Sort();
        ((List<OctreeLeaf>)this.Items).Reverse();
        sorted = true;
    }

    public bool LeafExists(OctreeLeaf leaf)
    {
        if (!sorted)
            Sort();
        if (levels[leaf.Level] == 0)
            return false;
        int minCounter = 0;
        int maxCounter = 0;
        for (int i = 0; i < leaf.Level; i++)
        {
            minCounter += levels[i];
        }
        maxCounter = minCounter + levels[leaf.Level];

        int currentCounter = (minCounter+maxCounter)/2;
        OctreeLeaf current = this[currentCounter];
        OctreeLeaf min = this[minCounter];
        OctreeLeaf max = this[maxCounter];
        if (min == leaf || max == leaf)
            return true;
        while (min<leaf&&max>leaf&&!current.Equals(leaf))
        {
            if(current > leaf)
            {
                maxCounter = currentCounter;
                max = current;
            }
            else
            {
                minCounter = currentCounter;
                min = current;
            }
            currentCounter = (minCounter + maxCounter) / 2;
            current = this[currentCounter];
        }
        return current.Equals(leaf);
    }
}

