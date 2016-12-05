namespace MVCClient.ViewModels.Helpers
{
    public class PrintViewModel
    {
        public int Id { get; set; }

        public string ServerName { get { return "SERVERCTY"; } }
        public string CatalogName { get { return "DemoPortals"; } }

        public string ReportFolder { get { return "DemoPortals"; } }
        public string ReportPath { get; set; }

        public int PrintOptionID { get; set; }
    }
}