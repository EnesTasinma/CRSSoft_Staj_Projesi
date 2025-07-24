# CRSSoft_Staj_Projesi
 Hukuki süreçlerde muhtemel senaryo tahmini (RAG)


# Hukuki Olay Senaryosu İçin Karar Destek Sistemi (RAG Tabanlı)

## Proje Tanımı

Bu proje, vatandaşların veya hukuk öğrencilerinin gerçek ya da kurgusal olayları sisteme anlatarak Türk Ceza Kanunu (TCK), Yargıtay kararları ve emsal davalardan **muhtemel sonuçları ve hukuki değerlendirmeleri öğrenmelerini sağlayan** bir yapay zeka destekli karar destek sistemidir.

Temel amaç, karmaşık hukuki süreçleri daha anlaşılır hale getirmek ve RAG (Retrieval-Augmented Generation) mimarisi ile olay-temelli bağlamsal bilgi üretmektir.

## Features

### Kullanıcı Özellikleri:
    + Serbest metinle olay anlatımı ("Ben birini bıçakladım, çünkü bana küfretti.")
    + Doğal dilde senaryo analizi
    + Muhtemel cezaların listelenmesi (TCK maddeleriyle)
    + Hakimin inisiyatif kullanabileceği alanların açıklanması (örneğin haksız tahrik, meşru müdafaa)
    + Benzer Yargıtay kararlarının özetlenmiş şekilde sunulması
    + Senaryoya benzer 3 emsal olayın gösterilmesi

### Geliştirici Özellikleri (Admin):
    + Yeni dava örnekleri ekleme (JSON/PDF ile)
    + Kanun maddesi güncellemeleri
    + Gelecekte API desteği ile güncel mevzuat entegrasyonu


##  Kullanılan Teknolojiler

**Backend**   **C# (.NET Core / ASP.NET Web API)**  
RESTful API ile kullanıcı olaylarını alır, embedding ve vektör arama yapar. Gerekirse dış LLM servisine bağlanır.  
HTTPClient, JSON serialization, middleware yapısı kullanılır.

 **Frontend** , **React.js (Vite veya Create React App)**  
Modern, hızlı ve kullanıcı dostu arayüz. Kullanıcı olaylarını yazar, sonuçları görüntüler.  
TailwindCSS veya Material UI ile stil verilebilir.

 **LLM**       **Gemma3:4B (Local)**  
Senaryo bazlı bağlamlı cevaplar üretir. Yerel olarak çalışan bir LLM sunucusu (örn. Ollama, LM Studio) üzerinden HTTP API ile çağrılır. Prompt ve kullanıcı girdi yönetimi C# üzerinden yapılır.

 **Embedding**  **OpenAI text-embedding-ada-002**  
Kullanıcı metni ve dava içerikleri embedlenir. .NET üzerinden doğrudan OpenAI embedding endpoint’ine bağlanılır.

 **Vector DB**  **Qdrant (gRPC veya REST ile bağlanılır)**  
Vektörel olarak indekslenmiş yasa maddeleri ve dava içerikleri burada tutulur. .NET için [Qdrant gRPC SDK](https://github.com/qdrant/qdrant-client-dotnet) veya REST endpoint kullanılabilir.

 **Veri İşleme**   
- PDF parsing: **PdfPig** veya **iTextSharp** (.NET için PDF okuma)  
- JSON işleme: **System.Text.Json**  
- Text preprocessing: Normal C# string utils / regex / tokenizer

 **Veri Formatları**   
- **PDF**: Dava metinleri ve iddianameler  
- **JSON**: Yasa maddeleri, karar özeti verisi  
- **Markdown (opsiyonel)**: Açıklayıcı hukuk yorumları

 **Yardımcı Kütüphaneler (.NET)**   
- `HttpClient` – API çağrıları için  
- `Newtonsoft.Json` – JSON parsing  
- `PdfPig` – PDF parsing  
- `Qdrant.Client` – Vektör DB entegrasyonu  
- `Swashbuckle` – Swagger dokümantasyonu  


##  Veri Kaynakları

 **Yargıtay Kararları**: kararara.yargitay.gov.tr üzerinden kamuya açık davalar

 **Türk Ceza Kanunu Maddeleri (TCK)**: mevzuat.gov.tr

 **CMK, TMK ve ilgili mevzuat** (Genişletilebilir)

 **Akademik yorum ve hukuk blog içerikleri** (isteğe bağlı)


## Sistem Akışı

1. Kullanıcı → Olay anlatımı → Backend  
2. Backend → Embedding üretimi → Qdrant arama  
3. Qdrant → İlgili içerikleri döner  
4. Backend → LLM'e bağlamlı prompt gönderir  
5. LLM → Yanıt üretir  
6. Backend → Cevabı frontend'e döner → Kullanıcı sonucu görür  

## Örnek kullanım

**Kullanıcı Girdisi** “Bana küfür eden komşumu sinirle bıçakladım ve olay yerinde hayatını kaybetti. Ne olur?”

**Rag Sistemi Yanıtı**

+Türk Ceza Kanunu Madde 81 (Kasten öldürme) kapsamında değerlendirilir.

+Ancak TCK 29’a göre “haksız tahrik” indirimi uygulanabilir.

+Yargıtay 1. CD 2020/XXXX kararında benzer bir olayda ceza indirimi uygulanmış.

+Ceza: 12–18 yıl arası olabilir. Takdir hakimi inisiyatifindedir.
