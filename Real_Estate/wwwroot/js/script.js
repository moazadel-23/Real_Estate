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
    const statsSection = document.querySelector('.stats');
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
document.addEventListener('DOMContentLoaded', () => {
    const menuBtn = document.querySelector('.menu-toggle');
    const mobileMenu = document.querySelector('.mobile-menu');

    if (menuBtn && mobileMenu) {
        menuBtn.addEventListener('click', () => {
            mobileMenu.classList.toggle('active');
        });
    }
});
