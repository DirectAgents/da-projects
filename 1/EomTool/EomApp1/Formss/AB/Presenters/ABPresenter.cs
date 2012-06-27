using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomApp1.Formss.AB.Views;
using EomApp1.Formss.AB.Model;

namespace EomApp1.Formss.AB.Presenters
{
    class ABPresenter
    {
        private IABView _view;
        private IABModel _model;
        internal void Init(IABView view)
        {
            _view = view;
            _model = new ABModel2();
            _view.Advertisers = _model.GetAdvertisers();
        }

        public string SelectedAdvertiser 
        { 
            set
            {
                bool convert = _view.ConvertToTargetCurrency;
                var abItems = _model.GetABItems(value, convert);
                _view.ABItems = abItems;
                if (convert)
                {
                    _view.Total = abItems.Sum(c => c.Total);
                }
            }
        }
    }
}
