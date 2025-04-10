export interface NavLink {
    label: string;
    route: string;
  }
  
  export function getUserNavLinks(userId: string | null): NavLink[] {
    return [
      userId
        ? { label: 'My Profile', route: '/profile'}//`/profile/${userId}` }
        : { label: 'Login', route: '/login' },
      { label: 'Search Guides', route: '/search' },
      { label: 'Create a Guide', route: '/create' },
      { label: 'About Us', route: '/about' },
    ];
  }

  export function getCmsNavLinks(userId: string | null): NavLink [] {
    return [
      { label: 'Main', route: '/cms' },
      { label: 'My Audio Guides', route: '/cms' },
      { label: 'My Places', route: '/cms/places' },
      { label: 'FAQ', route: '/cms/faq' }
    ]
  }
