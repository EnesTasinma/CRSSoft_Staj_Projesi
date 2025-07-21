# 📄 Gemma 4B + Ollama + AnythingLLM Kurulum ve Entegrasyon Süreci

Bu belge, Ollama üzerinden indirilen `gemma3:4b` modelinin local olarak nasıl çalıştırıldığını ve AnythingLLM ile nasıl entegre edildiğini adım adım açıklamaktadır. Amaç, local bir LLM’in nasıl serve edildiğini ve AnythingLLM'in bu LLM ile nasıl iletişim kurduğunu teknik düzeyde göstermektir.

---

## 🔹 1. LLM Kurulumu (Ollama ile)

### Ollama'nın Kurulumu  
Sistemime uygun Ollama kurulum paketi resmî web sitesinden indirildi ve yüklendi. Yükleme sonrası terminalden doğrulama yapıldı:

**ollama --version** komutu ile test edildi.

**ollama run gemma3:4b** Bu komut LLM local olarak varsa çalıştırıyor aksi takdirde bu LLM'i kuruyor.Bu komutla model http://localhost:11434 üzerinden HTTP API olarak çalışmaya başladı. Bu endpoint üzerinden yapılan her istek modele iletilir ve yanıt dönülür.

- Not: Ollama, modeli local olarak serve eder. Dış bir servise ihtiyaç duymadan kendi kendine çalışır.

## 🔹 2. Anything LLM Kurulumu

- AnythingLLM GitHub sayfasından indirildi. Sistem gereksinimleri karşılandıktan sonra uygulama başlatıldı ve tarayıcı üzerinden arayüz açıldı.

- Uygulama üzerinden `settings` kısmından LLM provider olarak Ollama seçildi. Local olarak kurulu olan Gemma3:4b seçildi. Bu ayarlarla Anything LLM Ollama'da çalışan modeli kullanmaya başladı.

## ❓ Sık Sorulan Sorulara Yanıtlar

**LLM’i nereden serve ediyorsun?**

→ Ollama üzerinden http://localhost:11434 adresinden serve ediyorum.

**AnythingLLM kendi içinde mi model çalıştırıyor?**

→ Hayır. Modeli çalıştıran Ollama. AnythingLLM sadece arayüz ve yönlendirme katmanıdır.

**3rd party LLM servisi kullandın mı?**

→ Hayır. Her şey local olarak çalışıyor. Dış bağlantı yok.


# Semantic Kernel + Ollama ile Lokal Soru-Cevap Uygulaması Geliştirme Süreci

## 📌 Geliştirme Ortamı
- Kod Geliştirme: macOS (MacBook)

- Model Sunucusu: Windows PC (Ollama üzerinde Gemma 3B)

### Kullanılan Teknolojiler:

- C# (.NET)

- Microsoft Semantic Kernel

- Ollama (Local LLM çalıştırmak için)

- Gemma 3B (Ollama üzerinden indirilen model)

- Basit terminal tabanlı soru-cevap senaryosu


## C# ve Semantic Kernel ile Test Uygulaması Geliştirme
MacBook üzerinde bir .NET projesi oluşturuldu. Amaç, temel bir soru-cevap etkileşimi sağlamaktı. Microsoft Semantic Kernel framework’ü kullanılarak LLM ile bağlantı sağlayacak bir chat yapısı kuruldu.

### Kod içinde şu adımlar yer aldı:

- Kernel nesnesi oluşturuldu.

- LLM bağlantısı için HttpClient kullanıldı.

- Kullanıcının giriş yaptığı prompt’lar, HTTP üzerinden LLM'e gönderildi.

- Dönen yanıtlar terminalde gösterildi.

## LLM Modelinin Windows Üzerinde Sunulması
LLM modeli olarak Gemma 3B kullanıldı. Bu model, Windows üzerinde kurulu olan Ollama platformu aracılığıyla serve edildi.

- Ollama Windows'a kuruldu.

- Terminalden aşağıdaki komutla model indirildi ve çalıştırıldı:

- ollama run gemma3:4b

Model, varsayılan olarak yalnızca localhost:11434 adresinden hizmet veriyordu. Fakat Mac'ten bu modele erişebilmek için aşağıdaki gibi tüm ağ arabirimlerinden erişime izin verecek şekilde Ollama yeniden başlatıldı:

- Ortam değişkenkerinden OLLAMA_HOST değişkeni `0.0.0.0` değeri ile oluşturuldu.

## Port Açma ve Güvenlik Ayarları

Model dışarıya açık hale getirilse bile, Windows’un varsayılan güvenlik duvarı politikaları nedeniyle dış bağlantılar engellenmişti. Bu nedenle şu işlemler yapıldı:

### Windows Güvenlik Duvarı > Gelişmiş Ayarlar > Gelen Kurallar menüsünden:

- 11434 TCP portu için özel bir inbound kural tanımlandı.

- Gerekirse firewall kısa süreli olarak devre dışı bırakıldı (sadece test için).

## Bağlantı Testleri
MacBook’tan Windows bilgisayara gerçekten bağlanılıp bağlanılamadığını test etmek için curl komutu kullanıldı:

- curl http://192.168.1.45:11434/api/generate -d '{"model": "gemma3:4b", "prompt": "Hello, how are you?", "stream": false}'

Eğer bu istek başarılı bir yanıt dönerse, bağlantının doğru şekilde kurulduğu teyit edildi.

