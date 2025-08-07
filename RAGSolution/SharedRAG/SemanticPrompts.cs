namespace SemanticKernelRAG
{
    public static class SemanticPrompts
    {
        public static string ClassifierPrompt = @"
        Sen bir hukuk sınıflandırma asistanısın. 
        Kullanıcının verdiği olayın hangi suç türüne girdiğini analiz et ve aşağıdaki suç türlerinden uygun olanları seç:

        - Kasten öldürme
        - Kasten yaralama
        - Dolandırıcılık
        - Hırsızlık
        - Cinsel saldırı

        Sadece aşağıdaki JSON formatında, veri olarak cevap ver:

        {
        ""suç_türü"": [""""],
        ""ceza_türü"": """",
        ""ceza_aralığı_yıl"": {
            ""min"": 0,
            ""max"": 0
        },
        ""tahrik"": false,
        ""iyi_hal"": false,
        ""meşru_müdafaa"": false,
        ""teşebbüs"": false,
        ""ek_not"": """"
        }

        Olay: {{$input}}
        ";

        public static string FinalResponsePrompt = @"
        Sen bir hukuk danışmanı değil, sadece JSON çıktısı sağlayan bir sınıflandırma sistemisin.
        Kullanıcının verdiği olayı aşağıdaki yargı kararlarını dikkate alarak değerlendir:

        YARGI KARARLARI:
        {{$context}}

        OLAY:
        {{$input}}

        Yalnızca aşağıdaki JSON formatında cevap ver:

        {
        ""suç_türü"": [""""],
        ""ceza_türü"": """",
        ""ceza_aralığı_yıl"": {
            ""min"": 0,
            ""max"": 0
        },
        ""tahrik"": false,
        ""iyi_hal"": false,
        ""meşru_müdafaa"": false,
        ""teşebbüs"": false,
        ""ek_not"": """"
        }
        ";

    }
}
