using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Genetic_Algorithm
{
    public class Node
    {
        public char Name { get; }
        public Point Position { get; }

        public Node(char name, Point position)
        {
            this.Name = name;
            this.Position = position;
        }
    }
    public class Gene : List<Node> {
        public double Fitness { get; set; }
        public bool isMutation = false;
        public int mutationNodeIndex1;
        public int mutationNodeIndex2;

        public void DeepCopy(Gene gene)
        {
            List<Node> nodeList = gene.ToList();
            this.Clear();
            this.AddRange(nodeList);

            this.Fitness = gene.Fitness;
            this.isMutation = gene.isMutation;
            this.mutationNodeIndex1 = gene.mutationNodeIndex1;
            this.mutationNodeIndex2 = gene.mutationNodeIndex2;
        }

        public void Reset()
        {
            this.DeepCopy(new Gene());
        }
    }

    internal class Genetic
    {
        // Inputable
        public int nodeCounts;                     // 유전자당 노드 수
        public int geneCounts;                     // 유전자 수
        private int mutationProbability;           // 돌연변이 발생 확률

        public int generation;                     // 현재 세대 수
        public double optimumFitness;              // 현재 세대 최적 적합도(노드간 거리 총합)
        public int bestGeneIndex;                  // 현재 세대 최적 유전자 위치

        public Gene originGene = [];               // 초기 유전자
        public List<Gene> transformGeneList = [];  // 변형 유전자 그룹
        public Gene bestGene = [];                 // 최적 유전자

        private List<int> rouletteWheel = [];      // 유전자 교배 룰렛

        private Random rand = new Random(DateTime.Now.Millisecond);

        public Genetic(int nodeCounts, int geneCounts, int mutationProbability)
        {
            this.nodeCounts = nodeCounts;
            this.geneCounts = geneCounts;
            this.mutationProbability = mutationProbability;

            reset();
        }

        // 초기화
        public void reset()
        {
            generation = 1;
            optimumFitness = 1000000.0;

            originGene.Reset();        // 초기 유전자 초기화
            transformGeneList.Clear(); // 변형 유전자 그룹 초기화
            bestGene.Reset();          // 최적 유전자 초기화

            generateOriginGene();        // 초기 유전자 생성
            generateTransformGeneList(); // 변형 유전자 그룹 생성
            calcuFitness();              // 변형 유전자 그룹 적합도 계산     

            bestGene.DeepCopy(originGene);
        }

        // 초기 유전자 생성
        private void generateOriginGene()
        {
            int x, y;
            char name;
            for (int i = 0; i < nodeCounts; i++)
            {
                x = rand.Next(10, 490);
                y = rand.Next(10, 490);
                name = (char)('A' + i);

                originGene.Add(new Node(name, new Point(x, y)));
            }
        }

        // 변형 유전자 그룹 생성
        private void generateTransformGeneList()
        {
            for (int i = 0; i < geneCounts; i++)
            {
                transformGeneList.Add(generateTransformGene());
            }
        }

        // 변형 유전자 생성
        private Gene generateTransformGene()
        {
            Gene gene = new Gene();
            gene.DeepCopy(originGene);

            // Fisher-Yates Shuffle: 노드 랜덤 정렬, 시작 노드는 제외 
            Shuffle(gene);

            return gene;
        }

        // Fisher-Yates Shuffle, 처음 원소는 고정
        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count - 1;
            while (n > 1)
            {
                n--;
                int k = rand.Next(1, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // 변형 유전자 그룹 적합도 계산  
        private void calcuFitness()
        {
            double sum = 0;

            foreach (Gene transformGene in transformGeneList)
            {
                // 노드 간 거리 총합 계산
                sum = 0;
                for (int i = 0; i < nodeCounts; i++)
                {
                    sum += distancePointToPoint(transformGene[i].Position, transformGene[i + 1 != nodeCounts ? i + 1 : 0].Position); 
                }

                // 노드 간 거리 총합 저장
                transformGene.Fitness = sum;
            }
        }

        // 2개 노드간 거리 계산
        private double distancePointToPoint(Point P1, Point P2)
        {
            // 좌표평면 상의 두점의 거리(피타고라스 정리)
            return Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2));
        }



        // 실행
        public void Operate()
        {
            makeRouletteWheel();  // 적합도(노드간 거리 총합) 작은순으로 가중치 둔 룰렛휠 만들기
            selection();          // 룰렛휠 돌려서 변형 유전자 그룹 선택
            calcuFitness();       // 변형 유전자 그룹 적합도 계산 
            searchBestGene();     // 최적 유전자 찾기
            generation++;         // 세대 증가
        }

        // 적합도(노드간 거리 총합) 작은순으로 가중치 둔 룰렛휠 만들기
        private void makeRouletteWheel()
        {
            rouletteWheel.Clear();

            List<double> fitnessList = [];
            transformGeneList.ForEach(transformGene => fitnessList.Add(transformGene.Fitness));

            int index, count = geneCounts;
            for(int i = 0; i < nodeCounts; i++)
            {
                index = fitnessList.IndexOf(fitnessList.Min());
                fitnessList[index] = 1000000;

                for (int j = 0; j < count; j++)
                {
                    rouletteWheel.Add(index);
                }

                count--;
            }
        }

        // 룰렛휠 돌려서 변형 유전자 그룹 선택
        private void selection()
        {
            Gene newGene = new Gene();
            List<Gene> newGeneGroup = new List<Gene>();
            int geneIndex1, geneIndex2;
            bool compare = false;

            for (int i = 0; i < geneCounts; i++)
            {
                // 룰렛휠에서 첫번째 유전자 선택
                geneIndex1 = rouletteWheel[rand.Next(0, rouletteWheel.Count)];

                // 룰렛휠에서 첫번째 유전자를 제외한 두번째 유전자 선택
                while (true)
                {
                    int temp = rand.Next(0, rouletteWheel.Count);
                    if (geneIndex1 != rouletteWheel[temp])
                    {
                        geneIndex2 = rouletteWheel[temp];
                        break;
                    }
                }

                // 유전자 교배
                newGene = crossover([transformGeneList[geneIndex1], transformGeneList[geneIndex2]]);

                // 유전자 중복 없이 삽입
                compare = false;
                foreach (Gene node in newGeneGroup)
                {
                    compare = Enumerable.SequenceEqual(node, newGene);
                    if (compare) break;
                }
                if (compare) i--; // 중복, 재시도
                else newGeneGroup.Add(newGene);
            }

            // 신규 변형 유전자 그룹 저장
            transformGeneList.Clear();
            transformGeneList.AddRange(newGeneGroup.ToList());
        }

        // 유전자 교배: 2개 유전자 간의 결합으로 새로운 자식 유전자 생성
        public Gene crossover(Gene[] geneList)
        {
            Gene child = new Gene();
            // 참조 끊기
            Gene gene1 = new Gene();
            Gene gene2 = new Gene();
            gene1.DeepCopy(geneList[0]);
            gene2.DeepCopy(geneList[1]);

            // 유전자 2분할 기준 위치 랜덤 지정
            int division = rand.Next(1, nodeCounts - 1);

            //  앞 부분: gene1
            for (int i = 0; i < division; i++)
            {
                child.Add(gene1[i]);
            }

            // 뒷 부분: gene2
            int index = 0;
            for (int i = division; i < nodeCounts; i++)
            {
                index = child.FindIndex(x => x.Name == gene2[i].Name);   // 동일한 노드가 있는지 확인 

                if (index < 0) child.Add(gene2[i]);                      // 동일한 노드가 없으면 추가
                else                                                     // 동일한 노드가 있으면 gene2의 앞 부분에서 없는 노드를 찾아 넣는다.
                {
                    for (int j = 0; j < division; j++)
                    {
                        index = child.FindIndex(x => x.Name == gene2[j].Name);
                        if (index < 0)
                        {
                            child.Add(gene2[j]);
                            break;
                        }
                    }
                }
            }

            // 돌연변이 생성
            mutation(child);

            return child;
        }

        // 돌연변이 생성: 유전자의 노드 위치를 변경하여 돌연변이 형성
        private void mutation(Gene gene)
        {
            int tickBasedValue = Environment.TickCount % 100; // 0~99
            int randomValue = rand.Next(100);                 // 0~99
            if ((tickBasedValue + randomValue) % 100 < mutationProbability)
            {
                // 1번 노드 랜덤 선택(시작 노드는 제외)
                int pos1 = rand.Next(1, nodeCounts);
                int pos2;

                // 1번 노드와 다른 위치로 2번 노드 랜덤 선택(시작 노드는 제외)
                while (true)
                {
                    pos2 = rand.Next(1, nodeCounts);
                    if (pos1 != pos2) break;
                }

                // 노드 위치 변경
                Gene tempGene = new Gene();
                tempGene.DeepCopy(gene);
                gene[pos1] = tempGene[pos2];
                gene[pos2] = tempGene[pos1];

                // 돌연변이 모니터링
                gene.isMutation = true;
                gene.mutationNodeIndex1 = pos1;
                gene.mutationNodeIndex2 = pos2;
            }
        }

        // 최적 유전자 찾기
        private void searchBestGene()
        {
            int index = 0, max_pos = 0;
            double max = 0;
            bool change = false;
            foreach (Gene transformGene in transformGeneList)
            {
                // 최적 유전자 저장
                if(transformGene.Fitness < optimumFitness)
                {
                    optimumFitness = transformGene.Fitness;
                    bestGene.DeepCopy(transformGeneList[index]);
                    bestGeneIndex = index; // 최적 유전자 모니터링
                    change = true;
                }

                // 최악 유전자 저장
                if(transformGene.Fitness > max)
                {
                    max = transformGene.Fitness;
                    max_pos = index;
                }

                index++;
            }

            // 최적 유전자 못 찾은 경우, 최악 유전자를 이전 세대 최적 유전자로 대체
            if (change == false)
            {
                transformGeneList[max_pos].DeepCopy(bestGene);
                bestGeneIndex = max_pos; // 최적 유전자 모니터링
            }
        }

    }
}
