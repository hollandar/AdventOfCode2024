using Command.Framework;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace Command.Problems._2024;


public partial class DiskFragmenter: ProblemBase<long>
{
    int[] diskMap = Array.Empty<int>();
    int[] disk = Array.Empty<int>();

    public DiskFragmenter()
    {
    }

    protected override void Line(string line)
    {
        Debug.Assert(disk.Length == 0);
        diskMap = line.ToCharArray().Select(c => c - '0').ToArray();

        List<int> diskList = new();
        int id = 0;
        for (int i = 0; i < diskMap.Length; i++)
        {
            var fileBlock = i % 2 == 0;
            var length = diskMap[i];

            if (length > 0)
            {
                if (fileBlock)
                {
                    diskList.AddRange(Enumerable.Repeat(id++, length));
                }
                else
                {
                    diskList.AddRange(Enumerable.Repeat(-1, length));
                }
            }
        }

        disk = diskList.ToArray();
    }


    public override long CalculateOne()
    {
        var defragmentDisk = disk.ToArray();
        while (!Unfragmented(defragmentDisk))
        {
            var indexedDisk = defragmentDisk.Index();
            var firstEmptyIndex = indexedDisk.Where(c => c.Item == -1).First();
            var lastFullIndex = indexedDisk.Where(c => c.Item != -1).Last();
            defragmentDisk[firstEmptyIndex.Index] = defragmentDisk[lastFullIndex.Index];
            defragmentDisk[lastFullIndex.Index] = -1;
        }

        return defragmentDisk.Index().Where(c => c.Item != -1).Sum(c => (long)c.Item * c.Index);
    }

    bool Unfragmented(int[] blocks)
    {
        var firstEmpty = Array.IndexOf(blocks, -1);
        if (firstEmpty == -1)
        {
            return true;
        }
        var lastFull = blocks.Index().Where(c => c.Item != -1).Select(r => r.Index).Last();
        return lastFull < firstEmpty;
    }

    record Block(int index, int item, int length);

    public override long CalculateTwo()
    {
        var fragmentedDisk = disk.ToArray();
        List<Block> blocks = new();

        // Convert the fragmented disk into a list of blocks
        int searchItem = fragmentedDisk[0];
        int length = 1;
        for (int i = 1; i < fragmentedDisk.Length; i++)
        {
            if (fragmentedDisk[i] == searchItem)
            {
                length++;
            }
            else
            {
                blocks.Add(new Block(i - length, searchItem, length));
                searchItem = fragmentedDisk[i];
                length = 1;
            }
        }
        blocks.Add(new Block(fragmentedDisk.Length - length, searchItem, length));

        var dataBlocks = blocks.Where(r => r.item != -1).Distinct().OrderByDescending(r => r.item).ToList();
        
        // Reverse through the blocks moving the ones we can
        foreach (var dataBlock in dataBlocks)
        {
            var emptyBlock = blocks.Where(r => r.item == -1 && r.length >= dataBlock.length && r.index < dataBlock.index).OrderBy(r => r.index).FirstOrDefault();
            if (emptyBlock is null) continue;

            var emptyBlockIndex = blocks.IndexOf(emptyBlock);
            blocks.RemoveAt(emptyBlockIndex);
            if (emptyBlock.length - dataBlock.length > 0)
            {
                blocks.Insert(emptyBlockIndex, new Block(emptyBlock.index + dataBlock.length, -1, emptyBlock.length - dataBlock.length));
            }
            blocks.Insert(emptyBlockIndex, new Block(emptyBlock.index, dataBlock.item, dataBlock.length));

            var dataBlockIndex = blocks.IndexOf(dataBlock);
            blocks.RemoveAt(dataBlockIndex);
            blocks.Insert(dataBlockIndex, new Block(dataBlock.index, -1, dataBlock.length));

            // Search for sequences of free blocks and consolidate them...
            for (int i = 0; i < blocks.Count - 1;)
            {
                if (blocks[i].item == -1 && blocks[i + 1].item == -1)
                {
                    var freeLength = blocks[i].length + blocks[i + 1].length;
                    blocks.RemoveAt(i + 1);
                    blocks[i] = new Block(blocks[i].index, -1, freeLength);
                }
                else
                {
                    i++;
                }
            }

        }

        // Recalculate the defragmented disk layout
        List<int> defragmentedDisk = new(blocks.Sum(r => r.length));
        foreach (var b in blocks)
        {
            defragmentedDisk.AddRange(Enumerable.Repeat(b.item, b.length));
        }

        return defragmentedDisk.Index().Where(r => r.Item != -1).Sum(r => (long)(r.Item * r.Index));
    }

    int BlockLength(int[] blocks, int startIndex)
    {
        var length = 0;
        var index = startIndex;
        while (index < blocks.Length && blocks[index] == blocks[startIndex])
        {
            length++;
            index++;
        }

        return length;
    }


}
