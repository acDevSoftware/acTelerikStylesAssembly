/*
 * Copyright (c) 2012 acDevSoftware
 * http://www.acdevsoftware.ch
 * 
 * This file is part of acTelerikStylesAssembly.
 *
 * acTelerikStylesAssembly is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.

 * acTelerikStylesAssembly is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.

 * You should have received a copy of the GNU Lesser General Public License
 * along with acTelerikStylesAssembly.  If not, see <http://www.gnu.org/licenses/>.
 * 
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acTelerikStylesAssembly
{
    public abstract class ProgressHandler
    {
        public delegate void ProgressProcessingHandler(object sender, ProgressProcessingEventArg e);
        public delegate void ProgressStatusHandler(object sender, ProgressStatusEventArg e);
        public event ProgressProcessingHandler ProgressProcessing;
        public event ProgressStatusHandler ProgressStatus;

        protected void OnProgressProcessing(ProgressProcessingEventArg e)
        {
            if (ProgressProcessing != null)
                ProgressProcessing(this, e);
        }

        protected void OnProgressStatus(ProgressStatusEventArg e)
        {
            if (ProgressStatus != null)
                ProgressStatus(this, e);
        }
    }
}
