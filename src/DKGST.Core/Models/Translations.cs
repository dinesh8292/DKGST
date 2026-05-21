namespace DKGST.Core.Models;

public class Translations
{
    public static Dictionary<string, Dictionary<string, string>> GetTranslations() => new()
    {
        {
            "en", new Dictionary<string, string>
            {
                // Common
                { "app_title", "DKGST - GST Accounting System" },
                { "login", "Login" },
                { "logout", "Logout" },
                { "dashboard", "Dashboard" },
                { "settings", "Settings" },
                { "profile", "Profile" },
                
                // Menu
                { "accounting", "Accounting" },
                { "inventory", "Inventory" },
                { "invoices", "Invoices" },
                { "gst_reports", "GST Reports" },
                { "journal", "Journal" },
                { "products", "Products" },
                { "stock", "Stock" },
                { "transfers", "Transfers" },
                { "adjustments", "Adjustments" },
                
                // Invoice
                { "invoice_number", "Invoice Number" },
                { "invoice_date", "Invoice Date" },
                { "customer_name", "Customer Name" },
                { "customer_gst", "Customer GST" },
                { "amount", "Amount" },
                { "total", "Total" },
                { "create_invoice", "Create Invoice" },
                { "edit_invoice", "Edit Invoice" },
                
                // Product
                { "product_name", "Product Name" },
                { "product_code", "Product Code" },
                { "category", "Category" },
                { "price", "Price" },
                { "quantity", "Quantity" },
                { "create_product", "Create Product" },
                
                // Actions
                { "save", "Save" },
                { "cancel", "Cancel" },
                { "delete", "Delete" },
                { "edit", "Edit" },
                { "view", "View" },
                { "search", "Search" },
                { "filter", "Filter" },
                { "export", "Export" },
                { "print", "Print" }
            }
        },
        {
            "es", new Dictionary<string, string>
            {
                // Common
                { "app_title", "DKGST - Sistema de Contabilidad GST" },
                { "login", "Iniciar sesión" },
                { "logout", "Cerrar sesión" },
                { "dashboard", "Panel de control" },
                { "settings", "Configuración" },
                { "profile", "Perfil" },
                
                // Menu
                { "accounting", "Contabilidad" },
                { "inventory", "Inventario" },
                { "invoices", "Facturas" },
                { "gst_reports", "Informes GST" },
                { "journal", "Diario" },
                { "products", "Productos" },
                { "stock", "Stock" },
                { "transfers", "Transferencias" },
                { "adjustments", "Ajustes" },
                
                // Invoice
                { "invoice_number", "Número de Factura" },
                { "invoice_date", "Fecha de Factura" },
                { "customer_name", "Nombre del Cliente" },
                { "customer_gst", "GST del Cliente" },
                { "amount", "Cantidad" },
                { "total", "Total" },
                { "create_invoice", "Crear Factura" },
                { "edit_invoice", "Editar Factura" },
                
                // Product
                { "product_name", "Nombre del Producto" },
                { "product_code", "Código del Producto" },
                { "category", "Categoría" },
                { "price", "Precio" },
                { "quantity", "Cantidad" },
                { "create_product", "Crear Producto" },
                
                // Actions
                { "save", "Guardar" },
                { "cancel", "Cancelar" },
                { "delete", "Eliminar" },
                { "edit", "Editar" },
                { "view", "Ver" },
                { "search", "Buscar" },
                { "filter", "Filtrar" },
                { "export", "Exportar" },
                { "print", "Imprimir" }
            }
        },
        {
            "hi", new Dictionary<string, string>
            {
                // Common
                { "app_title", "DKGST - जीएसटी लेखांकन प्रणाली" },
                { "login", "प्रवेश करें" },
                { "logout", "बाहर निकलें" },
                { "dashboard", "डैशबोर्ड" },
                { "settings", "सेटिंग्स" },
                { "profile", "प्रोफ़ाइल" },
                
                // Menu
                { "accounting", "लेखांकन" },
                { "inventory", "इन्वेंटरी" },
                { "invoices", "चालान" },
                { "gst_reports", "जीएसटी रिपोर्ट" },
                { "journal", "जर्नल" },
                { "products", "उत्पाद" },
                { "stock", "स्टॉक" },
                { "transfers", "स्थानांतरण" },
                { "adjustments", "समायोजन" },
                
                // Invoice
                { "invoice_number", "चालान संख्या" },
                { "invoice_date", "चालान तारीख" },
                { "customer_name", "ग्राहक का नाम" },
                { "customer_gst", "ग्राहक जीएसटी" },
                { "amount", "राशि" },
                { "total", "कुल" },
                { "create_invoice", "चालान बनाएं" },
                { "edit_invoice", "चालान संपादित करें" },
                
                // Product
                { "product_name", "उत्पाद का नाम" },
                { "product_code", "उत्पाद कोड" },
                { "category", "श्रेणी" },
                { "price", "कीमत" },
                { "quantity", "मात्रा" },
                { "create_product", "उत्पाद बनाएं" },
                
                // Actions
                { "save", "सहेजें" },
                { "cancel", "रद्द करें" },
                { "delete", "हटाएं" },
                { "edit", "संपादित करें" },
                { "view", "देखें" },
                { "search", "खोजें" },
                { "filter", "फ़िल्टर" },
                { "export", "निर्यात" },
                { "print", "प्रिंट" }
            }
        }
    };
}
