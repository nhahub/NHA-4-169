/**
 * BAYTACK — Customer App Shared Data Layer
 * ============================================================
 * Central mock "backend" for the whole Customer portal.
 * Everything is persisted in localStorage so the demo behaves
 * like a real, stateful app across page reloads/navigation.
 *
 * Keys used:
 *   ek_user_session      -> { phone, name, role }  (set by landing.js on login)
 *   bt_c_services        -> catalog of bookable services
 *   bt_c_providers       -> provider directory
 *   bt_c_requests        -> customer "Post a Request" items
 *   bt_c_orders          -> booked orders (from services or accepted offers)
 *   bt_c_saved           -> array of saved/favorited service ids
 *   bt_c_messages        -> conversations [{id, providerId, providerName, avatar, messages:[...]}]
 *   bt_c_notifications   -> notifications list
 * ============================================================
 */
(function (global) {
  'use strict';

  const KEYS = {
    services: 'bt_c_services',
    providers: 'bt_c_providers',
    requests: 'bt_c_requests',
    orders: 'bt_c_orders',
    saved: 'bt_c_saved',
    messages: 'bt_c_messages',
    notifications: 'bt_c_notifications',
    seeded: 'bt_c_seeded_v1',
  };

  function read(key, fallback) {
    try { return JSON.parse(localStorage.getItem(key)) ?? fallback; }
    catch (e) { return fallback; }
  }
  function write(key, value) {
    localStorage.setItem(key, JSON.stringify(value));
  }
  function uid(prefix) {
    return `${prefix}_${Date.now().toString(36)}${Math.random().toString(36).slice(2, 7)}`;
  }

  /* ── Session ── */
  function getSession() {
    try { return JSON.parse(localStorage.getItem('ek_user_session') || 'null'); }
    catch (e) { return null; }
  }
  function currentUser() {
    const s = getSession();
    return {
      name: (s && s.name) || 'Guest Customer',
      phone: (s && s.phone) || '',
      role: (s && s.role) || 'customer',
    };
  }

  /* ── Seed demo data (idempotent) ── */
  function seed() {
    if (read(KEYS.seeded, false)) return;

    const services = [
      { id: 'svc_1', title: 'Deep House Cleaning', category: 'Cleaning', icon: 'cleaning_services',
        provider: 'Sara Mohamed', providerId: 'prv_1', avatar: 'https://i.pravatar.cc/150?img=32',
        rating: 4.9, reviews: 425, price: 350, delivery: '1 day', location: 'Cairo',
        image: 'https://images.unsplash.com/photo-1581578731548-c64695cc6952?w=800',
        description: 'A full top-to-bottom deep clean using eco-friendly products, ideal for move-in/move-out or seasonal refresh.',
        features: ['Eco-friendly products', 'Kitchen & bathroom deep scrub', 'Window sills & baseboards', 'Trained & insured staff'],
        requirements: ['Access to water & electricity', 'Please clear fragile items beforehand'],
        packages: {
          basic: { name: 'Basic', price: 350, desc: 'Single room deep clean', delivery: '1 day' },
          standard: { name: 'Standard', price: 650, desc: 'Full apartment (up to 3 rooms)', delivery: '1 day' },
          premium: { name: 'Premium', price: 1100, desc: 'Full villa/large home + windows', delivery: '2 days' },
        } },
      { id: 'svc_2', title: 'Electrical Installation', category: 'Electrical', icon: 'electrical_services',
        provider: 'Mona Fathy', providerId: 'prv_2', avatar: 'https://i.pravatar.cc/150?img=47',
        rating: 4.8, reviews: 310, price: 250, delivery: '2 days', location: 'Giza',
        image: 'https://images.unsplash.com/photo-1621905252507-b35492cc74b4?w=800',
        description: 'Safe, code-compliant electrical installation and rewiring for homes and offices.',
        features: ['Licensed electrician', 'Safety inspection included', 'Panel & circuit upgrades'],
        requirements: ['Please shut off main power before arrival if possible'],
        packages: {
          basic: { name: 'Basic', price: 250, desc: 'Single fixture/outlet install', delivery: '1 day' },
          standard: { name: 'Standard', price: 550, desc: 'Room rewiring', delivery: '2 days' },
          premium: { name: 'Premium', price: 1400, desc: 'Full apartment rewiring', delivery: '4 days' },
        } },
      { id: 'svc_3', title: 'Plumbing Repair', category: 'Plumbing', icon: 'plumbing',
        provider: 'Omar Hassan', providerId: 'prv_3', avatar: 'https://i.pravatar.cc/150?img=15',
        rating: 4.9, reviews: 268, price: 180, delivery: 'Same day', location: 'Alexandria',
        image: 'https://images.unsplash.com/photo-1607400201889-565b1ee75c9c?w=800',
        description: 'Leak detection, pipe repair and general plumbing maintenance from a master plumber.',
        features: ['Leak detection', 'Pipe & valve replacement', '30-day workmanship guarantee'],
        requirements: ['Please keep the area under the sink accessible'],
        packages: {
          basic: { name: 'Basic', price: 180, desc: 'Single leak/fixture fix', delivery: 'Same day' },
          standard: { name: 'Standard', price: 400, desc: 'Multiple fixtures', delivery: '1 day' },
          premium: { name: 'Premium', price: 900, desc: 'Full bathroom re-plumb', delivery: '2 days' },
        } },
      { id: 'svc_4', title: 'AC Maintenance & Repair', category: 'AC Repair', icon: 'ac_unit',
        provider: 'Ahmed Ali', providerId: 'prv_4', avatar: 'https://i.pravatar.cc/150?img=13',
        rating: 4.8, reviews: 512, price: 300, delivery: 'Same day', location: 'Cairo',
        image: 'https://images.unsplash.com/photo-1631545806609-42f4c68cd645?w=800',
        description: 'Full AC service including gas refill, filter cleaning and cooling performance check.',
        features: ['Gas refill & leak check', 'Filter & coil cleaning', 'Performance report'],
        requirements: ['Please ensure AC unit is reachable'],
        packages: {
          basic: { name: 'Basic', price: 300, desc: 'Cleaning & inspection', delivery: 'Same day' },
          standard: { name: 'Standard', price: 550, desc: 'Cleaning + gas refill', delivery: 'Same day' },
          premium: { name: 'Premium', price: 950, desc: 'Full service, up to 3 units', delivery: '1 day' },
        } },
      { id: 'svc_5', title: 'Interior Painting', category: 'Painting', icon: 'format_paint',
        provider: 'Youssef Adel', providerId: 'prv_5', avatar: 'https://i.pravatar.cc/150?img=51',
        rating: 4.7, reviews: 189, price: 400, delivery: '3 days', location: 'Cairo',
        image: 'https://images.unsplash.com/photo-1562259949-e8e7689d7828?w=800',
        description: 'Clean, precise interior painting with premium washable paints and color consultation.',
        features: ['Color consultation', 'Surface prep & filling', 'Premium washable paint'],
        requirements: ['Please move furniture away from walls'],
        packages: {
          basic: { name: 'Basic', price: 400, desc: 'Single room', delivery: '1 day' },
          standard: { name: 'Standard', price: 900, desc: '3 rooms', delivery: '2 days' },
          premium: { name: 'Premium', price: 2200, desc: 'Full apartment', delivery: '4 days' },
        } },
      { id: 'svc_6', title: 'Custom Carpentry & Woodwork', category: 'Carpentry', icon: 'carpenter',
        provider: 'Karim Nabil', providerId: 'prv_6', avatar: 'https://i.pravatar.cc/150?img=60',
        rating: 4.9, reviews: 143, price: 500, delivery: '5 days', location: 'Tanta',
        image: 'https://images.unsplash.com/photo-1601058268499-e52658b8bb88?w=800',
        description: 'Custom-built furniture, shelving and finishing woodwork tailored to your space.',
        features: ['Custom design consultation', 'Premium hardwood options', 'On-site fitting'],
        requirements: ['Please provide rough measurements beforehand'],
        packages: {
          basic: { name: 'Basic', price: 500, desc: 'Small shelving unit', delivery: '3 days' },
          standard: { name: 'Standard', price: 1500, desc: 'Custom wardrobe', delivery: '5 days' },
          premium: { name: 'Premium', price: 3500, desc: 'Full room furnishing', delivery: '10 days' },
        } },
    ];

    const providers = services.map(s => ({
      id: s.providerId, name: s.provider, avatar: s.avatar, category: s.category,
      rating: s.rating, reviews: s.reviews, location: s.location,
      bio: `${s.provider} is a verified BayTack professional specializing in ${s.category.toLowerCase()}.`,
    }));

    const requests = [
      { id: uid('req'), title: 'Fix leaking kitchen faucet', category: 'Plumbing', budget: 300,
        deadline: daysFromNow(3), location: 'Tanta, Gharbia', status: 'open',
        description: 'The kitchen faucet has been dripping for a week, needs a proper fix or replacement.',
        images: [], skills: ['Plumbing', 'Faucet repair'], createdAt: daysAgo(1),
        offers: [
          { id: uid('off'), providerId: 'prv_3', providerName: 'Omar Hassan', avatar: 'https://i.pravatar.cc/150?img=15',
            rating: 4.9, price: 280, delivery: '1 day', message: 'I can fix this today, bringing my own parts.' },
          { id: uid('off'), providerId: 'prv_2', providerName: 'Mona Fathy', avatar: 'https://i.pravatar.cc/150?img=47',
            rating: 4.8, price: 320, delivery: '2 days', message: 'Available tomorrow morning, guaranteed work.' },
        ] },
      { id: uid('req'), title: 'Paint two bedrooms', category: 'Painting', budget: 1200,
        deadline: daysFromNow(10), location: 'Tanta, Gharbia', status: 'open',
        description: 'Need two bedrooms (roughly 3x4m each) repainted in a light neutral color.',
        images: [], skills: ['Painting', 'Interior finishing'], createdAt: daysAgo(2), offers: [] },
    ];

    const orders = [
      { id: uid('ord'), serviceId: 'svc_1', title: 'Deep House Cleaning', provider: 'Sara Mohamed',
        avatar: 'https://i.pravatar.cc/150?img=32', price: 650, status: 'active', progress: 60,
        createdAt: daysAgo(1), timeline: [
          { label: 'Order Placed', done: true }, { label: 'Provider Confirmed', done: true },
          { label: 'In Progress', done: true }, { label: 'Completed', done: false },
        ] },
      { id: uid('ord'), serviceId: 'svc_4', title: 'AC Maintenance & Repair', provider: 'Ahmed Ali',
        avatar: 'https://i.pravatar.cc/150?img=13', price: 300, status: 'completed', progress: 100,
        createdAt: daysAgo(12), timeline: [
          { label: 'Order Placed', done: true }, { label: 'Provider Confirmed', done: true },
          { label: 'In Progress', done: true }, { label: 'Completed', done: true },
        ] },
    ];

    const saved = ['svc_2', 'svc_5'];

    const messages = [
      { id: 'conv_1', providerId: 'prv_1', providerName: 'Sara Mohamed', avatar: 'https://i.pravatar.cc/150?img=32',
        lastMessage: 'See you tomorrow at 10 AM!', updatedAt: daysAgo(0),
        thread: [
          { from: 'provider', text: 'Hi! Confirming your cleaning appointment for tomorrow.', time: '9:12 AM' },
          { from: 'customer', text: 'Great, thank you! 10 AM works for me.', time: '9:20 AM' },
          { from: 'provider', text: 'See you tomorrow at 10 AM!', time: '9:21 AM' },
        ] },
      { id: 'conv_2', providerId: 'prv_3', providerName: 'Omar Hassan', avatar: 'https://i.pravatar.cc/150?img=15',
        lastMessage: 'I can fix this today, bringing my own parts.', updatedAt: daysAgo(1),
        thread: [
          { from: 'provider', text: 'I can fix this today, bringing my own parts.', time: 'Yesterday' },
        ] },
    ];

    const notifications = [
      { id: uid('ntf'), group: 'today', read: false, icon: 'local_shipping', title: 'Provider on the way',
        text: 'Sara Mohamed is heading to your cleaning appointment.', time: '2h ago' },
      { id: uid('ntf'), group: 'today', read: false, icon: 'local_offer', title: 'New offer received',
        text: 'You got a new offer on "Fix leaking kitchen faucet".', time: '4h ago' },
      { id: uid('ntf'), group: 'yesterday', read: true, icon: 'check_circle', title: 'Order completed',
        text: 'Your AC Maintenance order was marked as completed.', time: 'Yesterday' },
      { id: uid('ntf'), group: 'week', read: true, icon: 'star', title: 'Leave a review',
        text: 'How was your experience with Ahmed Ali?', time: '5 days ago' },
    ];

    write(KEYS.services, services);
    write(KEYS.providers, providers);
    write(KEYS.requests, requests);
    write(KEYS.orders, orders);
    write(KEYS.saved, saved);
    write(KEYS.messages, messages);
    write(KEYS.notifications, notifications);
    write(KEYS.seeded, true);
  }

  function daysAgo(n) { const d = new Date(); d.setDate(d.getDate() - n); return d.toISOString(); }
  function daysFromNow(n) { const d = new Date(); d.setDate(d.getDate() + n); return d.toISOString(); }
  function formatDate(iso) {
    try { return new Date(iso).toLocaleDateString('en-GB', { day: 'numeric', month: 'short', year: 'numeric' }); }
    catch (e) { return ''; }
  }

  /* ── Public API ── */
  const BTData = {
    KEYS, uid, currentUser, getSession, seed, formatDate,
    services: { all: () => read(KEYS.services, []), byId: id => read(KEYS.services, []).find(s => s.id === id) },
    providers: { all: () => read(KEYS.providers, []), byId: id => read(KEYS.providers, []).find(p => p.id === id) },
    requests: {
      all: () => read(KEYS.requests, []),
      byId: id => read(KEYS.requests, []).find(r => r.id === id),
      add: (req) => { const list = read(KEYS.requests, []); list.unshift(req); write(KEYS.requests, list); return req; },
      update: (id, patch) => {
        const list = read(KEYS.requests, []);
        const idx = list.findIndex(r => r.id === id);
        if (idx !== -1) { list[idx] = { ...list[idx], ...patch }; write(KEYS.requests, list); }
        return list[idx];
      },
      remove: (id) => write(KEYS.requests, read(KEYS.requests, []).filter(r => r.id !== id)),
    },
    orders: {
      all: () => read(KEYS.orders, []),
      byId: id => read(KEYS.orders, []).find(o => o.id === id),
      add: (order) => { const list = read(KEYS.orders, []); list.unshift(order); write(KEYS.orders, list); return order; },
      update: (id, patch) => {
        const list = read(KEYS.orders, []);
        const idx = list.findIndex(o => o.id === id);
        if (idx !== -1) { list[idx] = { ...list[idx], ...patch }; write(KEYS.orders, list); }
        return list[idx];
      },
    },
    saved: {
      all: () => read(KEYS.saved, []),
      isSaved: id => read(KEYS.saved, []).includes(id),
      toggle: (id) => {
        let list = read(KEYS.saved, []);
        if (list.includes(id)) list = list.filter(x => x !== id); else list.push(id);
        write(KEYS.saved, list);
        return list.includes(id);
      },
    },
    messages: {
      all: () => read(KEYS.messages, []),
      byId: id => read(KEYS.messages, []).find(c => c.id === id),
      send: (convId, text) => {
        const list = read(KEYS.messages, []);
        const idx = list.findIndex(c => c.id === convId);
        if (idx === -1) return;
        list[idx].thread.push({ from: 'customer', text, time: 'Just now' });
        list[idx].lastMessage = text;
        list[idx].updatedAt = new Date().toISOString();
        write(KEYS.messages, list);
      },
      unreadCount: () => 1, // demo constant badge
    },
    notifications: {
      all: () => read(KEYS.notifications, []),
      unreadCount: () => read(KEYS.notifications, []).filter(n => !n.read).length,
      markAllRead: () => {
        const list = read(KEYS.notifications, []).map(n => ({ ...n, read: true }));
        write(KEYS.notifications, list);
      },
      markRead: (id) => {
        const list = read(KEYS.notifications, []);
        const idx = list.findIndex(n => n.id === id);
        if (idx !== -1) { list[idx].read = true; write(KEYS.notifications, list); }
      },
    },
  };

  global.BTData = BTData;
  seed();
})(window);
