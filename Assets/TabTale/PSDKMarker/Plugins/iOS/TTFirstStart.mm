//
//  TTFirstStart.mm
//  Unity-iPhone
//
//  Created by Ariel Vardy on 22/07/2018.
//

#import "TTFirstStart.h"

@implementation TTFirstStart

extern "C" {
    
    void psdkMarkFirstRun() {
        
        // First check that we are running without PSDK.
        Class cls = NSClassFromString(@"PSDKServiceManager");
        
        if (cls != nil){
            NSLog(@"PSDK is inatlled plugin is disabled");
        }
        else{
            NSLog(@"PSDK is not inatlled plugin is enabled");
            if ([[NSUserDefaults standardUserDefaults] objectForKey:@"psdkFirstLaunch"] == nil){
                // In case this flag is not marked,  it means that this is first run and we should mark it.
                [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"psdkFirstLaunch"];
                [[NSUserDefaults standardUserDefaults] synchronize];
                NSLog(@"First session was marked");
            }
        }
    }
}

@end
