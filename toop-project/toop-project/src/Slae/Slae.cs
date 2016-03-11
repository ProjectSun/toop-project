﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toop_project.src.Slae {
    public class SLAE {
        src.GUI.IGUI iGui = null;
        public SLAE(src.GUI.IGUI igui) {
            iGui = igui;
        }

        public bool CanBeComputed() {
            if (Matrix == null) return false;
            if (Right == null) return false;
            // TODO: uncomment if need precond
            //if (Precond == null) return false;
            if (Solver == null) return false;

            // TODO: check sizes of rp & matrix?

            return true;
        }
        public void Solve() {
            //затычка, довести до запускаемой проги
            Preconditioner = src.Preconditioner.EmptyPreconditioner.Create(Matrix);

            Result = Solver.Solve(Preconditioner, Right, new src.Vector_.Vector(6), src.Logging.Logger.Instance, src.Logging.Logger.Instance,
                GenerateParameters(Solver.Type,MaxIter,Eps,Relaxation, MGMRES));
            iGui.FinishSolve();
        }
        public src.Matrix.BaseMatrix Matrix = null;
        public src.Vector_.Vector Right = null;
        public src.Vector_.Vector Result = null;
        public src.Solver.ISolver Solver = null;
        public src.Preconditioner.IPreconditioner Preconditioner = null;

        public double Eps = 1e-10;
        public int MaxIter = 1000;
        public double Relaxation = 1;
        public int MGMRES = 30;

        src.Solver.ISolverParametrs GenerateParameters(src.Solver.Type type, int maxIter, double eps, double relaxation = 1,int mGMRES = 5)
        {
            switch(type)
            {
                case src.Solver.Type.GMRES:
                    return new Solver.GMRESParameters(eps, maxIter, mGMRES);
                case src.Solver.Type.Jacobi:
                    return new Solver.JacobiParametrs(eps, maxIter, relaxation);
                case src.Solver.Type.Seidel:
                    return new Solver.GZParametrs(eps, maxIter, relaxation);
                case src.Solver.Type.MSG:
                    return new Solver.MSGParametrs(eps, maxIter);
            }
            throw new NotImplementedException();
        }
    }

}
