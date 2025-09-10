using DAL.Models;
using System;
using System.Linq.Expressions;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(int? branchId = null, string category = null, bool? isActive = null)
        : base(p =>
            (!branchId.HasValue || p.BranchID == branchId) &&
            (string.IsNullOrEmpty(category) || p.Category == category) &&
            (!isActive.HasValue || p.is_active == isActive)
        )
    { }
}
