using System.Drawing;
using System.Text;

namespace Genetic_Algorithm
{
    public partial class Form1 : Form
    {
        private Genetic genetic;

        public Form1()
        {
            // 유전자당 노드 수: 10, 유전자 수: 10, 돌연변이 확률: 1%(최대 100)
            genetic = new Genetic(10, 10, 1);

            InitializeComponent();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Pen pen = new Pen(Color.Red, 10);

            // draw point
            for (int i = 0; i < genetic.nodeCounts; i++)
            {
                Point pos = genetic.originGene[i].Position;
                graphics.DrawEllipse(pen, pos.X - 2 / 2, pos.Y - 2 / 2, 2, 2);

                pen.Color = Color.Green;
            }

            // draw line
            pen.Color = Color.Blue;
            pen.Width = 1;
            for (int i = 0; i < genetic.nodeCounts; i++)
            {
                graphics.DrawLine(pen, genetic.bestGene[i].Position, genetic.bestGene[i + 1 != genetic.nodeCounts ? i + 1 : 0].Position);
            }

            // set text
            labelFitness.Text = string.Format("Fitness : {0}", genetic.optimumFitness);
            labelGeneration.Text = string.Format("Generation : {0}", genetic.generation);

            // set text box
            textBox1.Clear();
            string name = "";
            for (int i = 0; i < genetic.nodeCounts; i++)
            {
                Gene transformGene = genetic.transformGeneList[i];

                // 최적 유전자 모니터링
                if(i == genetic.bestGeneIndex)
                {
                    name += "BEST: ";
                }

                // 돌연변이 모니터링
                if (transformGene.isMutation)
                {
                    int mutationNodeIndex1 = transformGene.mutationNodeIndex1;
                    int mutationNodeIndex2 = transformGene.mutationNodeIndex2;
                    name += string.Format("MUTATION({0},{1}): ", transformGene[mutationNodeIndex2].Name, transformGene[mutationNodeIndex1].Name);
                }
                
                genetic.transformGeneList[i].ForEach(node => name += node.Name);
                name += "\r\n";
            }
            textBox1.Text = name;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            genetic.reset();
            panel1.Refresh();
        }

        private void BtnOperate_Click(object sender, EventArgs e)
        {
            genetic.Operate();
            panel1.Refresh();
        }
    }
}