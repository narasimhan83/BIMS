// AEGIS IBMS - Language Switcher
// Handles switching between English and Arabic with RTL support

document.addEventListener('DOMContentLoaded', function() {
    // Load saved language preference
    const savedLang = localStorage.getItem('aegis-language') || 'en';
    setLanguage(savedLang, false);
    
    // Update active button
    updateActiveButton(savedLang);
});

function setLanguage(lang, reload = true) {
    // Save language preference
    localStorage.setItem('aegis-language', lang);
    
    // Update document direction and attributes
    const htmlElement = document.documentElement;
    const bodyElement = document.body;
    
    if (lang === 'ar') {
        htmlElement.setAttribute('dir', 'rtl');
        htmlElement.setAttribute('lang', 'ar');
        bodyElement.classList.add('arabic');
        bodyElement.classList.remove('english');
        
        // Load RTL CSS if not already loaded
        if (!document.getElementById('rtl-css')) {
            const link = document.createElement('link');
            link.id = 'rtl-css';
            link.rel = 'stylesheet';
            link.href = '/css/rtl.css';
            document.head.appendChild(link);
        }
    } else {
        htmlElement.setAttribute('dir', 'ltr');
        htmlElement.setAttribute('lang', 'en');
        bodyElement.classList.add('english');
        bodyElement.classList.remove('arabic');
        
        // Remove RTL CSS
        const rtlCss = document.getElementById('rtl-css');
        if (rtlCss) {
            rtlCss.remove();
        }
    }
    
    // Update active button state
    updateActiveButton(lang);
    
    // Save to server session via AJAX
    fetch('/Home/SetLanguage?lang=' + lang, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value
        }
    }).then(() => {
        if (reload) {
            // Reload page to apply language changes
            location.reload();
        }
    });
}

function updateActiveButton(lang) {
    // Remove active class from all buttons
    document.querySelectorAll('.lang-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    
    // Add active class to current language button
    const activeBtn = document.getElementById('btn-' + lang);
    if (activeBtn) {
        activeBtn.classList.add('active');
    }
}

// Event listeners for language buttons
document.addEventListener('click', function(e) {
    if (e.target.closest('.lang-btn-en')) {
        e.preventDefault();
        setLanguage('en', true);
    } else if (e.target.closest('.lang-btn-ar')) {
        e.preventDefault();
        setLanguage('ar', true);
    }
});

// Helper function to get current language
function getCurrentLanguage() {
    return localStorage.getItem('aegis-language') || 'en';
}

// Helper function to translate text (can be expanded)
function translate(key) {
    const translations = {
        'en': {
            'dashboard': 'Dashboard',
            'masters': 'Masters',
            'customerTypes': 'Customer Types',
            'documentTypes': 'Document Types',
            'businessTypes': 'Business Types',
            'welcome': 'Welcome',
            'logout': 'Logout'
        },
        'ar': {
            'dashboard': 'لوحة التحكم',
            'masters': 'الأساسيات',
            'customerTypes': 'أنواع العملاء',
            'documentTypes': 'أنواع المستندات',
            'businessTypes': 'أنواع الأعمال',
            'welcome': 'أهلاً',
            'logout': 'تسجيل الخروج'
        }
    };
    
    const currentLang = getCurrentLanguage();
    return translations[currentLang][key] || key;
}