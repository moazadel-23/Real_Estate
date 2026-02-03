// Properties Array (Now dynamic)
let allProperties = [];

// Fetch properties from API
async function loadProperties() {
    try {
        const response = await fetch('/api/properties');
        if (!response.ok) throw new Error('Failed to load data');
        allProperties = await response.json();
        renderProperties(allProperties);
    } catch (error) {
        console.error('Error loading properties:', error);
        // Fallback or alert
        document.getElementById('propertiesContainer').innerHTML = '<p style="text-align:center; color:red">فشل تحميل العقارات. يرجى المحاولة لاحقاً.</p>';
    }
}

// Initialize
// Initialize based on page
document.addEventListener('DOMContentLoaded', () => {
    if (window.location.pathname.includes('favorites.html')) {
        loadFavoritesPage();
    } else if (document.getElementById('propertiesContainer')) {
        // Only load main properties if container exists and NOT favorites page
        // (Though favorites page has container too, condition above catches it first)
        loadProperties();
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
                        <img src="${property.image}" alt="${property.title}">
                    </a>
                </div>
                <div class="card-content">
                    <div class="card-price">${property.price}</div>
                    <a href="property-details.html?id=${property.id}">
                        <h3 class="card-title">${property.title}</h3>
                    </a>
                    <div class="card-location">
                        <i class="fa-solid fa-location-dot"></i>
                        <span>${property.location}</span>
                    </div>
                <div class="card-features">
                    <div class="feature">
                        <i class="fa-solid fa-bed"></i>
                        <span>${property.beds}</span>
                    </div>
                    <div class="feature">
                        <i class="fa-solid fa-bath"></i>
                        <span>${property.baths}</span>
                    </div>
                    <div class="feature">
                        <i class="fa-solid fa-ruler-combined"></i>
                        <span>${property.area}م</span>
                    </div>
                </div>
                <div class="card-actions" style="margin-top: 15px; border-top: 1px solid #eee; padding-top: 15px; display: flex; gap: 10px; justify-content: center;">
                    <button class="btn btn-primary" style="padding: 8px 15px; font-size: 0.9rem;" onclick="addToCart(${property.id})">
                        <i class="fa-solid fa-cart-plus"></i> شراء
                    </button>
                    <a href="property-details.html?id=${property.id}" class="btn btn-outline" style="padding: 8px 15px; font-size: 0.9rem;">
                        <i class="fa-solid fa-eye"></i> تفاصيل
                    </a>
                </div>
            </div>
        `;
        propertiesContainer.appendChild(card);
    });
}

// Favorite Actions
window.toggleFavorite = function (id, event) {
    if (event) {
        event.stopPropagation();
        event.preventDefault();
    }

    let favorites = JSON.parse(localStorage.getItem('favorites') || '[]');
    const index = favorites.findIndex(p => p.id === id);

    if (index === -1) {
        // Add to favorites
        // Find property object from allProperties or current context
        let property = allProperties.find(p => p.id === id);

        // If not found in allProperties (e.g. maybe we are on a page where allProperties isn't fully loaded or different source),
        // try to find it in the favorites list itself if we were removing (already handled by index check),
        // or check if we can reconstruct it? No, we need the object.
        // Assuming allProperties is populated (it is on index.html).

        if (property) {
            favorites.push(property);
            localStorage.setItem('favorites', JSON.stringify(favorites));
            alert('تمت الإضافة للمفضلة');
        }
    } else {
        // Remove from favorites
        favorites.splice(index, 1);
        localStorage.setItem('favorites', JSON.stringify(favorites));
        alert('تم الحذف من المفضلة');

        // If on favorites page, re-render
        if (window.location.pathname.includes('favorites.html')) {
            renderProperties(favorites); // Re-use renderProperties
            // Check if empty
            if (favorites.length === 0) {
                document.getElementById('propertiesContainer').innerHTML = '<p style="grid-column: 1/-1; text-align: center;">لا توجد عقارات في المفضلة.</p>';
            }
        }
    }

    // Toggle icons
    const buttons = document.querySelectorAll(`.fav-btn-${id}`);
    buttons.forEach(btn => btn.classList.toggle('active'));
};

// Actions Functions
window.addToCart = function (id) {
    const property = allProperties.find(p => p.id === id);
    if (!property) return;

    let cart = JSON.parse(localStorage.getItem('cart') || '[]');
    // Check if already in cart
    if (cart.some(item => item.id === id)) {
        alert('هذا العقار موجود بالفعل في السلة');
        return;
    }

    cart.push(property);
    localStorage.setItem('cart', JSON.stringify(cart));
    alert('تمت إضافة العقار للسلة بنجاح');
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
    if (confirm('هل أنت متأكد من تسجيل الخروج؟')) {
        // Clear session/local storage if needed (optional)
        // localStorage.removeItem('userProfile'); // or keep it
        window.location.href = 'login.html'; // Redirect to login page
    }
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
