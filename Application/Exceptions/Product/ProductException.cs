using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Util;

namespace Application.Exceptions.Product
{
    public class ProductException : Exception
    {
        public ProductException(ProductMessageException message) : base(message.GetDescription()) { }
    }

    public enum ProductMessageException
    {
        [Description("Creation was not successed.")]
        CreateFailed = 0,
        [Description("Title is required")]
        TitleCanNotBeNull = 1,
        [Description("Product item can not be null.")]
        ItemCanNotBeNull = 3,
        [Description("Product does not exist")]
        DataNotFound = 4,
        [Description("Id can not be null")]
        IdIsRequired = 5,
        [Description("Null request is not acceptable")]
        RequestIsNull = 6,
        [Description("Product is already exist")]
        ProductAlreadyExist
    }
}
