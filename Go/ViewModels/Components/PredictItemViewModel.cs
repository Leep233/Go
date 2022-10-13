using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.ViewModels.Components
{
    public class PredictItemViewModel:BindableBase
    {
        private float predict;

        public float Predict
        {
            get { return predict; }
            set {
                predict = value;
                RaisePropertyChanged("Predict");
                Weight = (predict);
            }
        }

        private float weight;

        public float Weight
        {
            get { return weight; }
            set { weight = value; RaisePropertyChanged("Weight"); }
        }
        public PredictItemViewModel()
        {

        }
        public PredictItemViewModel(float pridict)
        {
            Predict = pridict;
        }
    }
}
