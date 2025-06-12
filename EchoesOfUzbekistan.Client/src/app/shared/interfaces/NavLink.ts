export interface NavLink {
    label: string;
    route: string;
  }
  
  export function getUserNavLinks(userId: string | null): NavLink[] {
    return [
      userId
        ? { label: 'My Profile', route: '/profile' } // or `/profile/${userId}`
        : { label: 'Login', route: '/login' },
      { label: 'Search Guides', route: '/search' },
      { label: 'Create a Guide', route: '/create' },
      { label: 'About Us', route: '/about' },
      // Add the logout link if user is logged in
      userId ? { label: 'Logout', route: '/logout' } : null
    ].filter(link => link !== null); // Remove any null entries (if user is not logged in)
  }
  
  export function getCmsNavLinks(userId: string | null): NavLink[] {
    return [
      { label: 'Main', route: '/cms' },
      { label: 'My Audio Guides', route: '/cms' },
      { label: 'My Places', route: '/cms/places' },
      { label: 'FAQ', route: '/cms/faq' },
      // Add the logout link for CMS mode
      userId ? { label: 'Logout', route: '/logout' } : null
    ].filter(link => link !== null); // Filter out null if user is not logged in
  }
