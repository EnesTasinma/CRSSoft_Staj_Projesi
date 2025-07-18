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



