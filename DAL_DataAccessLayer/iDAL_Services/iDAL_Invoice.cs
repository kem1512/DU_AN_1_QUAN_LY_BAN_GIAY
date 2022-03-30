﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_DataAccessLayer.Entities;

namespace DAL_DataAccessLayer.iDAL_Services
{
    public interface iDAL_Invoice
    {
        string AddInvoice(Invoice invoice, InvoiceDetail invoiceDetail);
        string UpdateInvoice(Invoice invoice, InvoiceDetail invoiceDetail);
        string RemoveInvoice(string id);
        List<Invoice> GetInvoices();
        List<Invoice> GetInvoicesDetail();
        Invoice GetInvoiceById(string id);
    }
}
