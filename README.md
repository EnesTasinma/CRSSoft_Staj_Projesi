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


## Kullanılan Teknolojiler

 **Backend**  Ben python bilmediğimden ötürü java ile yapmayı düşünüyorum. Ama Python'ın kullanım kolaylıklarından en büyüğü RAG istemi için popüler olan teknolojilerden biri olan llama_index kütüphanesini içerirken javada böyle bir kütüphane yok. Java'da "embedding, chunking veya vektör arama" işlemlerini kendimiz manuel yazmamız gerekir. Java da python kadar kolay bir şekilde embedding ve LLM entegrasyonu yapılamıyor. Ve nadir kullanıldığı için herhangi bir sorunla karşılaştığımızda form veya dökümaantaston eksikliği yaşanıyor.

 **Frontend**  Streamlit (hızlı prototipleme): kullanıcı gelip direkt sorusunu sorduktan sonra cevabını öğrenip gider. İleride kalıcı veya daha geniş çaplı ürünler için react kullanılabilir.

 **Vector DB**  FAISS (offline): Kullanıcıdan gelen verileri chunklayıp vektörler hâlinde saklar. Gelen soruyu da çaklayıp en yakın vektörle cevap verir.

 **LLM / RAG Servisi** | Python (Flask/FastAPI) | LangChain + FAISS + GPT-3.5

 **Embedding**  OpenAI `text-embedding-ada-002`

 **LLM**  OpenAI GPT-3.5 Turbo (API üzerinden): Bağlamlı yanıt üretici.

 **Veri İşleme**  LangChain, Pandas, PyMuPDF 

 **Veri Formatları**  JSON, Markdown, PDF 

 **Yardımcı Kütüphaneler**  tiktoken, pydantic, dotenv, etc.
 
 **Python kullanımı**
 - langchain → text splitting, retrieval

 - faiss → vektör tabanlı arama

 - openai SDK → GPT-3.5 ile yanıt üretme

 - PyMuPDF veya pdfplumber → dava dosyası/iddianame PDF parsing


##  Veri Kaynakları

 **Yargıtay Kararları**: kararara.yargitay.gov.tr üzerinden kamuya açık davalar
 **Türk Ceza Kanunu Maddeleri (TCK)**: mevzuat.gov.tr
 **CMK, TMK ve ilgili mevzuat** (Genişletilebilir)
 **Akademik yorum ve hukuk blog içerikleri** (isteğe bağlı)


## Sistem Akışı

Kullanıcı Girişi (Frontend)
↓
Spring Boot REST API (Java)
↓
POST → Python LLM Servisi
↓
Embedding + FAISS + Prompting
↓
LLM → Yanıt oluşturur
↓
Yanıt Java’ya döner → Kullanıcıya gösterilir

**Kullanıcı Girdisi** “Bana küfür eden komşumu sinirle bıçakladım ve olay yerinde hayatını kaybetti. Ne olur?”

**Python Rag Sistemi Yanıtı**
Türk Ceza Kanunu Madde 81 (Kasten öldürme) kapsamında değerlendirilir.

Ancak TCK 29’a göre “haksız tahrik” indirimi uygulanabilir.

Yargıtay 1. CD 2020/XXXX kararında benzer bir olayda ceza indirimi uygulanmış.

Ceza: 12–18 yıl arası olabilir. Takdir hakimi inisiyatifindedir.
