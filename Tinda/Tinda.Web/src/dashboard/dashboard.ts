import {Router, RouterConfiguration} from 'aurelia-router'

export class dashboard {
    heading = 'Dashboard';
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.map([
        { 
            route: ['', 'dashboard'],   
            name: 'dashboard',       
            moduleId: 'dashboard',       
            nav: true, 
            title: 'Dashboard' 
        },
        { 
            route: 'new-customer-order',             
            name: 'new-customer-order',         
            moduleId: 'new-customer-order',         
            nav: true, 
            title: 'New Customer Order' 
        },
        { 
            route: 'new-purchase-order',  
            name: 'new-purchase-order',  
            moduleId: 'new-purchase-order',  
            nav: true, 
            title: 'New Purchase Order' 
        }
    ]);

    this.router = router;
  }
}
