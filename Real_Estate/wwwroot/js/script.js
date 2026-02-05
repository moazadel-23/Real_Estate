// Properties Array (Now dynamic)
let allProperties = [];

// Fetch properties from API
// Fetch properties from LocalStorage (Mock API)
async function loadProperties() {
    try {
        // Simulate API delay
        // await new Promise(resolve => setTimeout(resolve, 500));

        const storedProperties = JSON.parse(localStorage.getItem('properties') || '[]');

        if (storedProperties.length === 0) {
            // Default Initial Data if empty (optional demo data)
            allProperties = [
                {
                    id: 101,
                    Title: "شقة فاخرة بإطلالة بانورامية",
                    Price: "2,500,000 ر.س",
                    location: "الرياض - الصحافة", // Keeping location as it might be used somewhere else or I can map it too? User didn't ask to change location key but I'll stick to requested ones for display.
                    Bedrooms: 3,
                    baths: 2,
                    Floor: 3,
                    AreaSize: 180,
                    MainImg: "https://images.unsplash.com/photo-1545324418-cc1a3fa10c00?ixlib=rb-1.2.1&auto=format&fit=crop&w=400&q=80",
                    category: "apartment",
                    Description: "شقة بتصميم مودرن في أرقى أحياء الرياض، تتميز بإطلالة خلابة وتشطيبات سوبر ديلوكس. قريبة من الخدمات الرئيسية.",
                    IsActive: true
                },
                {
                    id: 102,
                    Title: "فيلا مودرن في حي الياسمين",
                    Price: "4,200,000 ر.س",
                    location: "الرياض - الياسمين",
                    Bedrooms: 5,
                    baths: 6,
                    Floor: 0,
                    AreaSize: 450,
                    MainImg: "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-1.2.1&auto=format&fit=crop&w=400&q=80",
                    category: "villa",
                    Description: "فيلا مستقلة بمسبح خاص وحديقة واسعة، تصميم ذكي يستغل المساحات بشكل مثالي. تشطيب vip.",
                    IsActive: true
                },
                {
                    id: 103,
                    Title: "مكتب إداري في التحلية",
                    Price: "1,800,000 ر.س",
                    location: "الرياض - التحلية",
                    Bedrooms: 0,
                    baths: 1,
                    Floor: 5,
                    AreaSize: 120,
                    MainImg: "https://images.unsplash.com/photo-1497366216548-37526070297c?ixlib=rb-1.2.1&auto=format&fit=crop&w=400&q=80",
                    category: "office",
                    Description: "مكتب جاهز ومؤثث بالكامل في موقع استراتيجي حيوي، مناسب للشركات الناشئة ورواد الأعمال.",
                    IsActive: true
                }
            ];
            // Save defaults so we have data to see
            // localStorage.setItem('properties', JSON.stringify(allProperties));
        } else {
            allProperties = storedProperties;
        }

        renderProperties(allProperties);
    } catch (error) {
        console.error('Error loading properties:', error);
        document.getElementById('propertiesContainer').innerHTML = '<p style="text-align:center; color:red">فشل تحميل العقارات.</p>';
    }
}

// Initialize
// Initialize based on page
document.addEventListener('DOMContentLoaded', () => {
    if (window.location.pathname.includes('favorites.html')) {
        loadFavoritesPage();
    } else if (document.getElementById('propertiesContainer')) {
        // loadProperties(); // Disabled to allow static HTML from Index
    }
});

function loadFavoritesPage() {
    const favorites = JSON.parse(localStorage.getItem('favorites') || '[]');
    if (favorites.length === 0) {
        const container = document.getElementById('propertiesContainer');
        if (container) container.innerHTML = '';
        const emptyMsg = document.getElementById('emptyMsg');
        if (emptyMsg) emptyMsg.style.display = 'block';
    } else {
        renderProperties(favorites);
    }
}

const propertiesContainer = document.getElementById('propertiesContainer');

function renderProperties(props) {
    if (!propertiesContainer) return;

    propertiesContainer.innerHTML = '';

    const favorites = JSON.parse(localStorage.getItem('favorites') || '[]');

    props.forEach(property => {
        // Check IsActive if it exists
        if (property.IsActive === false) return;

        // Handle both new and old keys for backward compatibility/graceful degradation if needed,
        // but prioritizing new keys as requested.
        const title = property.Title || property.title;
        const price = property.Price || property.price;
        const image = property.MainImg || property.image;
        const floor = property.Floor !== undefined ? property.Floor : property.floor;
        const description = property.Description || property.description;
        const bedrooms = property.Bedrooms !== undefined ? property.Bedrooms : property.beds;
        const areaSize = property.AreaSize || property.area;

        const isFav = favorites.some(p => p.id === property.id);
        const card = document.createElement('div');
        card.classList.add('property-card');
        card.innerHTML = `
                <div class="card-image">
                    <button class="card-fav-btn ${isFav ? 'active' : ''} fav-btn-${property.id}" onclick="toggleFavorite(${property.id}, event)">
                        <i class="fa-solid fa-heart"></i>
                    </button>
                    <a href="property-details.html?id=${property.id}">
                        <span class="card-badge">${property.category === 'villa' ? 'فيلا' : 'شقة'}</span>
                        <img src="${image}" alt="${title}">
                    </a>
                </div>
                <div class="card-content">
                    <div class="card-header" style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px;">
                        <div class="card-price" style="font-size: 1.2rem; font-weight: bold; color: var(--accent-color);">${price}</div>
                        <span class="card-floor" style="font-size: 0.8rem; color: #777;"><i class="fa-solid fa-layer-group"></i> الدور: ${floor !== undefined ? floor : 'الأرضي'}</span>
                    </div>
                    <a href="property-details.html?id=${property.id}">
                        <h3 class="card-title">${title}</h3>
                    </a>
                    <p class="card-description" style="font-size: 0.9rem; color: #666; margin-bottom: 15px; display: -webkit-box; -webkit-line-clamp: 2; -webkit-box-orient: vertical; overflow: hidden;">
                        ${description || 'هذا العقار يتميز بموقع استراتيجي وتصميم عصري يناسب جميع الأذواق. تواصل معنا لمزيد من التفاصيل.'}
                    </p>
                    
                    <div class="card-features">
                        <div class="feature" title="غرف النوم">
                            <i class="fa-solid fa-bed"></i>
                            <span>${bedrooms} غرف</span>
                        </div>
                        <div class="feature" title="المساحة">
                            <i class="fa-solid fa-ruler-combined"></i>
                            <span>${areaSize} م²</span>
                        </div>
                    </div>

                    <div class="card-actions" style="margin-top: 15px; border-top: 1px solid #eee; padding-top: 15px; display: flex; gap: 10px; justify-content: center;">
                        <button class="btn btn-primary" style="padding: 8px 15px; font-size: 0.9rem; flex: 1;" onclick="addToCart(${property.id})">
                            <i class="fa-solid fa-cart-plus"></i> شراء
                        </button>
                        <a href="property-details.html?id=${property.id}" class="btn btn-outline" style="padding: 8px 15px; font-size: 0.9rem; flex: 1; text-align: center;">
                            <i class="fa-solid fa-eye"></i> تفاصيل
                        </a>
                    </div>
                </div>
        `;
        propertiesContainer.appendChild(card);
    });
}

// Favorite Actions
// Favorite Actions
window.toggleFavorite = function (id, event) {
    if (event) {
        event.stopPropagation();
        event.preventDefault();
    }

    // Toggle icons for visual effect only
    const buttons = document.querySelectorAll(`.fav-btn-${id}`);
    buttons.forEach(btn => btn.classList.toggle('active'));

    // Optional: Visual Confirmation
    const isActive = buttons[0]?.classList.contains('active');
    alert(isActive ? 'تمت الإضافة للمفضلة (واجهة فقط)' : 'تم الحذف من المفضلة (واجهة فقط)');
};

// Actions Functions
window.addToCart = function (id) {
    alert('تمت إضافة العقار للسلة بنجاح (واجهة فقط)');
};

// Initial Render
// Initial Render is now handled by loadProperties()

// Navbar Scroll Effect
window.addEventListener('scroll', () => {
    const navbar = document.querySelector('.navbar');
    if (window.scrollY > 50) {
        navbar.style.background = 'rgba(255, 255, 255, 0.98)';
        navbar.style.padding = '0';
        navbar.style.boxShadow = '0 5px 20px rgba(0,0,0,0.1)';
    } else {
        navbar.style.background = 'rgba(255, 255, 255, 0.9)';
        navbar.style.padding = '10px 0';
        navbar.style.boxShadow = 'none';
    }
});

// Counter Animation
const counters = document.querySelectorAll('.counter');
const speed = 200;

const animateCounters = () => {
    counters.forEach(counter => {
        const target = +counter.getAttribute('data-target');
        const count = +counter.innerText;
        const inc = target / speed;

        if (count < target) {
            counter.innerText = Math.ceil(count + inc);
            setTimeout(animateCounters, 20);
        } else {
            counter.innerText = target;
        }
    });
};

// Trigger animation when stats section is in view
let animated = false;
window.addEventListener('scroll', () => {
    const statsSection = document.querySelector('.stats-container');
    if (statsSection) {
        const sectionTop = statsSection.offsetTop;
        const scrollY = window.scrollY;
        if (scrollY > sectionTop - 500 && !animated) {
            animateCounters();
            animated = true;
        }
    }
});

// Mobile Menu Toggle Logic
// Sidebar Menu Toggle
function toggleSidebarMenu() {
    const sidebar = document.querySelector('.sidebar-wrapper');
    if (sidebar) {
        sidebar.classList.toggle('active');
    }
}

// Mobile Menu Trigger
document.addEventListener('DOMContentLoaded', () => {
    const menuBtn = document.querySelector('.menu-toggle');
    if (menuBtn) {
        menuBtn.addEventListener('click', toggleSidebarMenu);
    }
});


// Profile Dropdown Logic
function toggleProfileDropdown() {
    const dropdown = document.getElementById('profileDropdown');
    dropdown.classList.toggle('active');
}

// Close Dropdown when clicking outside
document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('profileDropdown');
    const profileImg = document.querySelector('.nav-profile-img');

    // If click is outside dropdown AND outside the profile image
    if (dropdown && profileImg && !dropdown.contains(event.target) && !profileImg.contains(event.target)) {
        dropdown.classList.remove('active');
    }
});

// Update Nav Profile Image from LocalStorage
document.addEventListener('DOMContentLoaded', () => {
    const savedProfile = JSON.parse(localStorage.getItem('userProfile'));
    const navProfilePic = document.getElementById('navProfilePic');
    if (savedProfile && savedProfile.image && navProfilePic) {
        navProfilePic.src = savedProfile.image;
    }
});

function logout() {
    // Clear session/local storage if needed (optional)
    // localStorage.removeItem('userProfile'); // or keep it
    window.location.href = 'login.html'; // Redirect to login page
}


// Desktop Sidebar Collapse Logic
// Desktop Sidebar Toggle Logic (Open/Close)
function toggleDesktopSidebar() {
    const sidebar = document.querySelector('.sidebar-wrapper');
    const body = document.body;

    if (sidebar) {
        sidebar.classList.toggle('sidebar-open');
        body.classList.toggle('sidebar-open');

        // Save preference
        const isOpen = sidebar.classList.contains('sidebar-open');
        localStorage.setItem('sidebarOpen', isOpen);
    }
}

// Restore Sidebar State on Load
document.addEventListener('DOMContentLoaded', () => {
    // Default closed, but check storage
    const isOpen = localStorage.getItem('sidebarOpen') === 'true';
    if (isOpen) {
        const sidebar = document.querySelector('.sidebar-wrapper');
        const body = document.body;
        if (sidebar && window.innerWidth >= 992) {
            sidebar.classList.add('sidebar-open');
            body.classList.add('sidebar-open');
        }
    }
});

// Theme Toggle Logic
function toggleTheme() {
    const body = document.body;
    body.classList.toggle('dark-mode');

    const isDark = body.classList.contains('dark-mode');
    localStorage.setItem('darkMode', isDark);

    updateThemeIcon(isDark);
}

function updateThemeIcon(isDark) {
    const btn = document.getElementById('themeToggle');
    if (btn) {
        if (isDark) {
            btn.innerHTML = '<i class="fa-solid fa-sun"></i>';
            btn.style.color = 'var(--accent-color)';
        } else {
            btn.innerHTML = '<i class="fa-solid fa-moon"></i>';
            btn.style.color = 'var(--primary-color)';
        }
    }
}

// Check Theme on Load
document.addEventListener('DOMContentLoaded', () => {
    const isDark = localStorage.getItem('darkMode') === 'true';
    if (isDark) {
        document.body.classList.add('dark-mode');
        updateThemeIcon(true);
    }
});

// Language Toggle Logic
const translations = {
    ar: {
        nav_home: "الرئيسية",
        nav_properties: "العقارات",
        nav_favorites: "المفضلة",
        nav_services: "خدماتنا",
        nav_contact: "اتصل بنا",
        nav_admin: "لوحة الادمن",
        hero_title: "اكتشف منزل أحلامك <br> بتصميم عصري وفخامة لا تضاهى",
        hero_subtitle: "نوفر لك أفضل الخيارات العقارية في أرقى الأحياء السكنية",
        btn_search: '<i class="fa-solid fa-magnifying-glass"></i> بحث',
        search_placeholder: "ابحث عن عقار..."
    },
    en: {
        nav_home: "Home",
        nav_properties: "Properties",
        nav_favorites: "Favorites",
        nav_services: "Services",
        nav_contact: "Contact",
        nav_admin: "Admin Panel",
        hero_title: "Discover Your Dream Home <br> Modern Design & Luxury",
        hero_subtitle: "We provide the best real estate options in the finest areas.",
        btn_search: '<i class="fa-solid fa-magnifying-glass"></i> Search',
        search_placeholder: "Search for properties..."
    }
};

function toggleLanguage() {
    const currentLang = localStorage.getItem('lang') || 'ar';
    const newLang = currentLang === 'ar' ? 'en' : 'ar';
    localStorage.setItem('lang', newLang);
    updateLanguageUI(newLang);
}

function updateLanguageUI(lang) {
    const isEnglish = lang === 'en';

    // Update HTML attributes
    document.documentElement.lang = lang;
    document.documentElement.dir = isEnglish ? 'ltr' : 'rtl';

    // Update Text Content
    const elements = document.querySelectorAll('[data-i18n]');
    elements.forEach(el => {
        const key = el.getAttribute('data-i18n');
        if (translations[lang][key]) {
            // Check if HTML or Text
            if (key === 'btn_search' || key === 'hero_title') {
                el.innerHTML = translations[lang][key];
            } else {
                el.innerText = translations[lang][key];
            }
        }
    });

    // Update Placeholder if exists
    const searchInput = document.querySelector('.search-input');
    if (searchInput && translations[lang]['search_placeholder']) {
        searchInput.placeholder = translations[lang]['search_placeholder'];
    }

    // Update Toggle Button Text
    const langBtn = document.getElementById('langToggle');
    if (langBtn) {
        langBtn.innerText = isEnglish ? 'AR' : 'EN';
    }
}

// Initial Language Load
document.addEventListener('DOMContentLoaded', () => {
    const savedLang = localStorage.getItem('lang');
    if (savedLang) {
        updateLanguageUI(savedLang);
    }
});
