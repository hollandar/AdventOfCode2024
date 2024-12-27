using Command.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Command.Problems._2024
{
    public partial class CodeChronicle : ProblemBase<int>
    {
        List<int[]> locks = new();
        List<int[]> keys = new();
        public CodeChronicle()
        {
        }

        int inLock = -1;
        int inKey = -1;
        int[] lockProfile = [0,0,0,0,0]; 
        int[] keyProfile = [0,0,0,0,0];
        protected override void Line(string line)
        {
            if (String.IsNullOrWhiteSpace(line))
            {
                if (inLock != -1)
                {
                    locks.Add(lockProfile);
                }
                if (inKey != -1)
                {
                    keys.Add(keyProfile.Select(r => r - 1).ToArray());
                }
                inLock = -1;
                inKey = -1;
                lockProfile = [0, 0, 0, 0, 0];
                keyProfile = [0, 0, 0, 0, 0];
                return;
            }

            if (inLock >= 0)
            {
                for (int i = 0; i < lockProfile.Length; i++)
                {
                    lockProfile[i] += line[i] == '#' ? 1 : 0;
                }
                inLock++;
                return;
            }

            if (inKey >= 0)
            {
                for (int i = 0; i < keyProfile.Length; i++)
                {
                    keyProfile[i] += line[i] == '#' ? 1 : 0;
                }
                inKey++;
                return;
            }

            if (line.All(r => r == '#'))
            {
                inLock = 0;
                return;
            }

            if (line.All(r => r == '.'))
            {
                inKey = 0;
                return;
            }
        }

        public override int CalculateOne(bool exampleData)
        {
            FinishLoading();

            var count = 0;
            foreach (var lockProfile in locks) {
                foreach (var keyProfile in keys)
                {
                    var match = true;
                    for (int i = 0; i < lockProfile.Length;i++)
                    {
                        if (lockProfile[i] + keyProfile[i] > 5)
                        {
                            match = false;
                        }
                    }

                    count += match ? 1 : 0;
                }

            }
            return count;
        }

        private void FinishLoading()
        {
            if (inLock != -1)
            {
                locks.Add(lockProfile);
            }
            if (inKey != -1)
            {
                keys.Add(keyProfile.Select(r => r - 1).ToArray());
            }
        }

        public override int CalculateTwo(bool exampleData)
        {
            return default!;
        }


    }
}
