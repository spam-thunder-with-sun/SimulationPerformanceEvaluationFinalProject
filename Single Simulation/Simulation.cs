﻿using MathNet.Numerics.Random;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticSimulation
{
    internal class Simulation
    {
        readonly MersenneTwister mersenneTwister;
        double creatureCounter;
        readonly double crowdingCoefficient;
        readonly double deathProbability;
        readonly double reproductionProbability;

        public Simulation(short nCreatureStart, double reproductionProbability, double deathProbability, 
            double crowdingCoefficient, int seed)
        {
            // PRNG initialization
            mersenneTwister = new MersenneTwister(seed);

            //Parameters initialization
            this.creatureCounter = nCreatureStart;
            this.reproductionProbability = reproductionProbability;
            this.deathProbability = deathProbability;
            this.crowdingCoefficient = crowdingCoefficient;
        }

        public short NextIteration()
        {
            if(creatureCounter > 0)
            {
                short deltaCreature = 0;

                //Create a list of random numbers
                double[] probabilities = mersenneTwister.NextDoubles(2 * (short)creatureCounter);
                for (int i = 0; i < creatureCounter; ++i)
                {
                    if (probabilities[(i << 1)] < reproductionProbability)
                        ++deltaCreature;

                    if (probabilities[(i << 1) + 1] < deathProbability + (crowdingCoefficient * creatureCounter))
                        --deltaCreature;
                }

                //Update creature counter
                creatureCounter += deltaCreature;
            }

            return (short)creatureCounter;
        }

        public short NextIteration_New()
        {
            short deltaCreature = 0;

            //Create a list of random numbers
            double[] probabilities = mersenneTwister.NextDoubles((short)creatureCounter);
            double addCreatureProbability = reproductionProbability;
            double removeCreatureProbability = deathProbability + (crowdingCoefficient * creatureCounter);
            double nothingProbability = (1 - addCreatureProbability) * (1 - removeCreatureProbability);

            double summmedProbability = addCreatureProbability + removeCreatureProbability + nothingProbability;
            for (int i = 0; i < creatureCounter; ++i)
                if (probabilities[i] * summmedProbability < addCreatureProbability)
                    ++deltaCreature;
                else if (probabilities[i] * summmedProbability < addCreatureProbability + removeCreatureProbability)
                    --deltaCreature;

            //Update creature counter
            creatureCounter += deltaCreature;

            return (short)creatureCounter;
        }
    }
}