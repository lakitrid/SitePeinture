!function() {
    "use strict";
    angular.module("admin", [ "ngRoute", "theme", "ngDialog" ]).controller("AdminController", [ "$rootScope", "$scope", "$http", "ngDialog", "ThemeService", "$location", function(a, b, c, d, e, f) {
        a.IdentityService.isAuth().then(function() {
            a.isConnected === !1 && f.path("login");
        }), a.currentView = "admin", b.home = !1, b.event = !1, b.theme = !1, b.painting = !1, 
        b.pass = !1, b.paints = [], b.themeService = e, b.events = [], b.homeSuccess = !1, 
        b.order = "Title", b.reverse = !1;
        var g = function() {
            c.get("service/painting").then(function(a) {
                b.paints = a.data;
            }), b.themeService.Load(), c.get("service/event").then(function(a) {
                b.events = a.data;
            });
        };
        a.$on("ngDialog.closing", function(a, b) {
            g();
        }), g(), b.Add = function() {
            d.open({
                template: "EditPainting",
                controller: "EditPaintingController"
            });
        }, b.Edit = function(a) {
            d.open({
                template: "EditPainting",
                controller: "EditPaintingController",
                data: a
            });
        }, b.Delete = function(a, e) {
            b.confirm = d.openConfirm({
                template: "ConfirmDelete",
                controller: "ConfirmDeleteController",
                className: "ngdialog-theme-default confirm-dialog-theme"
            }).then(function() {
                c["delete"]("service/painting/" + a.Id), g();
            });
        }, b.AddTheme = function() {
            d.open({
                template: "EditTheme",
                controller: "EditThemeController"
            });
        }, b.EditTheme = function(a) {
            d.open({
                template: "EditTheme",
                controller: "EditThemeController",
                data: a
            });
        }, b.DeleteTheme = function(a, e) {
            b.confirm = d.openConfirm({
                template: "ConfirmDelete",
                controller: "ConfirmDeleteController",
                className: "ngdialog-theme-default confirm-dialog-theme"
            }).then(function() {
                c["delete"]("service/theme/" + a.Id), g();
            });
        }, b.AddEvent = function() {
            d.open({
                template: "EditEvent",
                controller: "EditEventController"
            });
        }, b.EditEvent = function(a) {
            d.open({
                template: "EditEvent",
                controller: "EditEventController",
                data: a
            });
        }, b.DeleteEvent = function(a, e) {
            b.confirm = d.openConfirm({
                template: "ConfirmDelete",
                controller: "ConfirmDeleteController",
                className: "ngdialog-theme-default confirm-dialog-theme"
            }).then(function() {
                c["delete"]("service/event/" + a.Id), g();
            });
        }, b.Reload = function() {
            g();
        }, b.Order = function(a) {
            b.order === a ? b.reverse = !b.reverse : (b.order = a, b.reverse = !1);
        }, b.IsOrder = function(a) {
            return a === b.order ? b.reverse ? "fa fa-caret-down pull-right" : "fa fa-caret-up pull-right" : null;
        };
    } ]).controller("AdminHomeController", [ "$scope", "$http", function(a, b) {
        a.article = {
            text: ""
        };
        var c = function() {
            b.get("service/home").then(function(b) {
                a.article.text = b.data;
            });
        };
        c(), a.SaveHomeArticle = function() {
            a.homeSuccess = !1, a.homeForm.$valid && b.post("service/home", JSON.stringify(a.article.text)).then(function() {
                a.homeSuccess = !0, c();
            });
        }, a.Reload = function() {
            c();
        };
    } ]).controller("EditPaintingController", [ "$scope", "$http", function(a, b) {
        angular.isDefined(a.ngDialogData) ? (a.isEdit = !0, a.paint = angular.copy(a.ngDialogData)) : (a.paint = {}, 
        a.isEdit = !1), a.themes = [], b.get("service/theme").then(function(b) {
            a.themes = b.data, a.themes.forEach(function(b) {
                angular.isDefined(a.paint) && b.Id == a.paint.ThemeId && (a.paint.currentTheme = b);
            });
        }), a.Save = function(c) {
            if (a.paintingForm.$valid) {
                var d = angular.copy(c);
                if (angular.isDefined(c.file)) d.FileName = c.file.Filename, d.Data = c.file.Data; else if (!a.isEdit) return;
                d.ThemeId = a.paint.currentTheme.Id, b.post("service/painting", d).then(function() {
                    a.closeThisDialog();
                });
            }
        };
    } ]).controller("EditThemeController", [ "$scope", "$http", function(a, b) {
        angular.isDefined(a.ngDialogData) ? (a.isEdit = !0, a.theme = angular.copy(a.ngDialogData)) : (a.isEdit = !1, 
        a.theme = {
            Id: 0
        }), a.parentThemes = [], b.get("service/theme/parents/" + a.theme.Id).then(function(b) {
            a.parentThemes = b.data, a.parentThemes.forEach(function(b) {
                b.Id == a.theme.ParentId && (a.theme.currentParent = b);
            });
        }), a.Save = function(c) {
            a.themeForm.$valid && (angular.isDefined(a.theme.currentParent) && (c.ParentId = a.theme.currentParent.Id), 
            b.post("service/theme", c).then(function() {
                a.closeThisDialog();
            }));
        };
    } ]).controller("EditEventController", [ "$scope", "$http", function(a, b) {
        angular.isDefined(a.ngDialogData) ? (a.isEdit = !0, a.event = angular.copy(a.ngDialogData)) : (a.isEdit = !1, 
        a.event = {
            Id: 0
        }), a.Save = function(c) {
            a.eventForm.$valid && b.post("service/event", c).then(function() {
                a.closeThisDialog();
            });
        };
    } ]).controller("ConfirmDeleteController", [ "$scope", "$http", function(a, b) {} ]).controller("ChangePasswordController", [ "$scope", "$http", function(a, b) {
        a.user = {}, a.error = {
            hasError: !1,
            errors: []
        }, a.hasSuccess = !1, a.ChangePassword = function() {
            a.hasSuccess = !1, a.passForm.$valid && (a.user.NewPassword === a.user.ConfirmPassword ? b.post("service/user/change", a.user).then(function(b) {
                a.error.hasError = !1, a.hasSuccess = !0, a.user = {};
            }, function(b) {
                angular.isDefined(b.data) && (a.error.hasError = !0, a.error.errors = b.data);
            }) : (a.error.hasError = !0, a.error.errors = [ "Le nouveau mot de passe et sa confirmation doivent être identiques" ]));
        };
    } ]);
}(), function() {
    "use strict";
    angular.module("appPeinture", [ "ngRoute", "ngDialog", "technical", "admin", "theme", "slider", "contact", "painting", "ngAnimate", "ngMaterial", "datePicker" ]).config([ "$routeProvider", "ngDialogProvider", "$httpProvider", function(a, b, c) {
        a.when("/home", {
            templateUrl: "views/home.html",
            controller: "HomeController"
        }).when("/contact", {
            templateUrl: "views/contact.html",
            controller: "ContactController"
        }).when("/theme/:themeId", {
            templateUrl: "views/theme.html",
            controller: "ThemeController"
        }).when("/painting/:paintingId", {
            templateUrl: "views/painting.html",
            controller: "PaintingController"
        }).when("/admin", {
            templateUrl: "views/admin.html",
            controller: "AdminController"
        }).when("/login", {
            templateUrl: "views/login.html",
            controller: "LoginController"
        }).otherwise({
            redirectTo: "/home"
        }), b.setDefaults({
            className: "ngdialog-theme-default fullsize-theme-default",
            showClose: !0,
            closeByDocument: !1,
            closeByEscape: !1
        }), c.interceptors.push("SiteHttpInterceptor");
    } ]).run([ "$rootScope", "$location", function(a, b) {
        a.connection = {
            count: 0
        };
        var c = "edumortier.fr" === b.host() || "www.edumortier.fr" === b.host();
        c && (ga("create", "UA-71336595-1", "auto"), a.$on("$routeChangeSuccess", function(a, c, d) {
            ga("send", "pageview", b.url());
        }));
    } ]).controller("MainController", [ "$rootScope", "$location", "$scope", "$http", "IdentityService", function(a, b, c, d, e) {
        a["goto"] = function(a) {
            b.path(a);
        }, a.isConnected = !1, a.IdentityService = e;
    } ]).controller("HomeController", [ "$scope", "$rootScope", "$http", function(a, b, c) {
        b.IdentityService.isAuth(), b.currentView = "home", a.homeArticle = "", c.get("service/home").then(function(b) {
            a.homeArticle = b.data;
        }), a.nextEvents = [], c.get("service/event/next").then(function(b) {
            a.nextEvents = b.data;
        });
    } ]).controller("MenuController", [ "$scope", "$http", "$location", "ThemeService", function(a, b, c, d) {
        a.paintingMenu = !1, a.themeService = d, a.themeService.Load(), a.display = function(b) {
            b === !0 && a.paintingMenu === !0 ? a.paintingMenu = !b : a.paintingMenu = b;
        }, a.gotoPainting = function(a) {
            c.path("theme/" + a.Id);
        };
    } ]);
}(), function() {
    "use strict";
    angular.module("contact", [ "ngRoute" ]).controller("ContactController", [ "$rootScope", "$scope", "$http", function(a, b, c) {
        a.IdentityService.isAuth(), a.currentView = "contact", b.hasSuccess = !1, b.hasError = !1, 
        b.contact = {}, b.Send = function() {
            b.hasSuccess = !1, b.hasError = !1, b.contactForm.$valid && c.post("service/contact", b.contact).then(function(a) {
                b.hasSuccess = !0, b.contact = {};
            }, function(a) {
                b.hasError = !0;
            });
        };
    } ]);
}(), function() {
    "use strict";
    angular.module("painting", [ "ngRoute" ]).controller("PaintingController", [ "$scope", "$http", "$routeParams", "$location", "BreadcrumbService", function(a, b, c, d, e) {
        a.paint = {}, b.get("service/painting/" + c.paintingId).then(function(c) {
            a.paint = c.data, angular.isDefined(a.paint) && b.get("service/theme/" + a.paint.ThemeId).then(function(b) {
                var c = [], d = b.data;
                angular.isDefined(d) && (0 !== d.ParentId && c.push({
                    label: d.ParentTitle,
                    hasTarget: !0,
                    target: "/theme/" + d.ParentId
                }), c.push({
                    label: d.Title,
                    hasTarget: !0,
                    target: "/theme/" + d.Id
                })), c.push({
                    label: a.paint.Title,
                    hasTarget: !1
                }), e.setElements(c);
            });
        });
    } ]);
}(), function() {
    "use strict";
    angular.module("slider", [ "ngRoute", "ngAnimate" ]).directive("slider", [ "$timeout", "$http", function(a, b) {
        return {
            restrict: "AE",
            replace: !0,
            scope: {},
            link: function(c, d, e) {
                c.paints = [], b.get("service/painting/slider").then(function(a) {
                    c.paints = a.data, c.currentIndex = 0;
                }), c.next = function(b) {
                    c.currentIndex = (c.currentIndex + 1) % c.paints.length, b || (a.cancel(f), f = a(g, 4e3));
                }, c.prev = function(b) {
                    c.currentIndex > 0 ? c.currentIndex-- : c.currentIndex = c.paints.length - 1, a.cancel(f), 
                    f = a(g, 4e3);
                }, c.$watch("currentIndex", function() {
                    c.paints.forEach(function(a) {
                        a.visible = !1;
                    }), c.paints.length > 0 && (c.paints[c.currentIndex].visible = !0);
                });
                var f, g = function() {
                    f = a(function() {
                        c.next(!0), f = a(g, 4e3);
                    }, 4e3);
                };
                g(), c.$on("$destroy", function() {
                    a.cancel(f);
                });
            },
            templateUrl: "views/slider.html"
        };
    } ]);
}(), function() {
    "use strict";
    angular.module("technical", []).factory("SiteHttpInterceptor", [ "$q", "appVersion", "$rootScope", function(a, b, c) {
        return {
            request: function(d) {
                if (c.connection.count++, 0 === d.url.search(/^(\.?\/)?(css|app|views|scripts)\//)) {
                    var e = d.url.indexOf("?") > -1 ? "&" : "?";
                    d.url = d.url + e + b;
                } else if (0 === d.url.search(/^(\.?\/)?(service)\//)) {
                    var e = d.url.indexOf("?") > -1 ? "&" : "?";
                    d.url = d.url + e + new Date().getTime();
                }
                return d || a.when(d);
            },
            response: function(b) {
                return c.connection.count--, b || a.when(b);
            },
            responseError: function(b) {
                return c.connection.count--, b || a.when(b);
            }
        };
    } ]).directive("markdown", [ function() {
        return {
            link: function(a, b, c) {
                var d = new showdown.Converter();
                a.$watch(function() {
                    return c.markdown;
                }, function(a, e) {
                    angular.isDefined(c.markdown) && (b[0].innerHTML = d.makeHtml(c.markdown));
                });
            }
        };
    } ]).controller("LoginController", [ "$scope", "$http", "$location", function(a, b, c) {
        a.user = {}, a.error = !1, a.Login = function() {
            a.error = !1, a.loginForm.$valid && b.post("service/user/login", a.user).then(function(b) {
                b.data === !0 ? c.path("/admin") : a.error = !0;
            });
        };
    } ]).factory("IdentityService", [ "$rootScope", "$http", "$location", function(a, b, c) {
        var d = function() {
            var c = b.get("service/user/isAuth");
            return c.then(function(b) {
                b.data === !0 ? a.isConnected = !0 : a.isConnected = !1;
            }, function(b) {
                a.isConnected = !1;
            }), c;
        }, e = function() {
            var d = b.get("service/user/signOut").then(function() {
                a.isConnected = !1, c.path("home");
            });
            return d;
        };
        return {
            isAuth: d,
            signOut: e
        };
    } ]).directive("fileread", [ function() {
        return {
            scope: {
                fileread: "="
            },
            link: function(a, b, c) {
                b.bind("change", function(b) {
                    var c = new FileReader();
                    c.onload = function(c) {
                        a.$apply(function() {
                            var d = {
                                Filename: b.target.files[0].name,
                                Data: c.target.result
                            };
                            a.fileread = d;
                        });
                    }, c.readAsDataURL(b.target.files[0]);
                });
            }
        };
    } ]).controller("BreadCrumbController", [ "$scope", "$location", "BreadcrumbService", function(a, b, c) {
        a.breadcrumbService = c, a.gotoElement = function(a) {
            b.path(a.target);
        };
    } ]).factory("BreadcrumbService", function() {
        var a = [];
        return {
            getElements: function() {
                return a;
            },
            setElements: function(b) {
                a = b, a.unshift({
                    label: "Accueil",
                    hasTarget: !0,
                    target: "/home"
                });
            }
        };
    });
}(), function() {
    "use strict";
    angular.module("theme", [ "ngRoute" ]).controller("ThemeController", [ "$scope", "$http", "$routeParams", "$location", "BreadcrumbService", "$interval", "themeInterval", function(a, b, c, d, e, f, g) {
        a.stop, a.pause = !1, a.theme = {}, a.subthemes = [], a.paints = [], b.get("service/theme/" + c.themeId).then(function(b) {
            a.theme = b.data;
            var c = [];
            angular.isDefined(a.theme) && (0 !== a.theme.ParentId && c.push({
                label: a.theme.ParentTitle,
                hasTarget: !0,
                target: "/theme/" + a.theme.ParentId
            }), c.push({
                label: a.theme.Title,
                hasTarget: !1
            })), e.setElements(c);
        }), b.get("service/theme/subthemes/" + c.themeId).then(function(b) {
            a.subthemes = b.data;
        }), b.get("service/painting/theme/" + c.themeId).then(function(b) {
            a.paints = b.data, angular.isDefined(a.paints) && a.paints.length > 0 && (a.index = 0, 
            a.theme.WithText || i());
        }), a.prev = function() {
            0 == a.index ? a.index = a.paints.length - 1 : a.index--, h();
        }, a.next = function() {
            a.index > a.paints.length - 2 ? a.index = 0 : a.index++, h();
        }, a.select = function(b) {
            a.index = b, h();
        }, a.setPause = function() {
            a.pause = !0, j();
        }, a.restart = function() {
            a.pause = !1, i();
        };
        var h = function() {
            a.pause || a.theme.WithText || (j(), i());
        }, i = function() {
            angular.isUndefined(a.stop) && !a.theme.WithText && (a.stop = f(function() {
                a.next();
            }, g));
        }, j = function() {
            angular.isDefined(a.stop) && (f.cancel(a.stop), a.stop = void 0);
        };
        a.gotoTheme = function(a) {
            d.path("theme/" + a.Id);
        }, a.gotoThemeId = function(a) {
            d.path("theme/" + a);
        }, a.gotoPaint = function(a) {
            d.path("painting/" + a);
        }, a.$on("$destroy", function() {
            j();
        });
    } ]).service("ThemeService", [ "$http", function(a) {
        var b = [], c = [], d = function() {
            a.get("service/theme").then(function(a) {
                b = a.data;
            }), a.get("service/theme/parents").then(function(a) {
                c = a.data;
            });
        };
        return {
            Load: d,
            getThemes: function() {
                return b;
            },
            getFirstLevelThemes: function() {
                return c;
            }
        };
    } ]);
}();