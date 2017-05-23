using System;
using System.Threading.Tasks;
using NT.Core.SharedKernel;
using NT.CustomerService.Infrastructure;
using NT.CustomerService.Core;

namespace NT.CustomerService.Migrator
{
    public static class CustomerContextSeeder
    {
        public static async Task Seed(CustomerDbContext dbContext)
        {
            dbContext.Set<Customer>().Add(new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Tuan",
                LastName = "Manh",
                ContactTitle  ="Mr",
                AddressInfo = new AddressInfo(Guid.NewGuid(), "123 ABC", "Ho Chi Minh", "", "", "VN"),
                ContactInfo = new ContactInfo(Guid.NewGuid(), "123456789", "1234567890", "http://tuanmanh.com.vn")
            });
            dbContext.Set<Customer>().Add(new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Thang",
                LastName = "Chung",
                ContactTitle = "Mr",
                AddressInfo = new AddressInfo(Guid.NewGuid(), "456 ABC", "Ho Chi Minh", "", "", "VN"),
                ContactInfo = new ContactInfo(Guid.NewGuid(), "123456789", "1234567890", "http://thangchung.com.vn")
            });
            await dbContext.SaveChangesAsync();
        }
    }
}