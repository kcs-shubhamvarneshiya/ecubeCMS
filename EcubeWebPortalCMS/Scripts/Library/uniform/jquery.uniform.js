/*

Uniform v1.8.0
Copyright © 2009 Josh Pyles / Pixelmatrix Design LLC
http://pixelmatrixdesign.com

Requires jQuery 1.3 or newer

Much thanks to Thomas Reynolds and Buck Wilson for their help and advice on this

Disabling text selection is made possible by Mathias Bynens <http://mathiasbynens.be/>
and his noSelect plugin. <http://github.com/mathiasbynens/noSelect-jQuery-Plugin>

Also, thanks to David Kaneda and Eugene Bond for their contributions to the plugin

License:
MIT License - http://www.opensource.org/licenses/mit-license.php

Enjoy!

*/

(function (d, i) {
    d.uniform = {
        options: {
            selectClass: "selector",
            radioClass: "radio",
            checkboxClass: "checker",
            fileClass: "uploader",
            filenameClass: "filename",
            fileBtnClass: "action",
            fileDefaultText: "",
            fileBtnText: "Choose File",
            checkedClass: "checked",
            focusClass: "focus",
            disabledClass: "disabled",
            buttonClass: "button",
            activeClass: "active",
            hoverClass: "hover",
            useID: !0,
            idPrefix: "uniform",
            resetSelector: !1,
            autoHide: !0,
            selectAutoWidth: !1
        },
        elements: []
    };
    d.support.selectOpacity =
        d.browser.msie && 7 > d.browser.version ? !1 : !0;
    d.fn.uniform = function (b) {
        function j(a) {
            var c = d("<div>"),
                e = d("<span>");
            c.addClass(b.buttonClass);
            b.useID && a.attr("id") && c.attr("id", b.idPrefix + "-" + a.attr("id"));
            var f;
            a.is("a, button") ? f = a.text() : a.is(":submit, :reset, input[type=button]") && (f = a.attr("value"));
            f = "" == f ? a.is(":reset") ? "Reset" : "Submit" : f;
            e.text(f);
            a.css("display", "none");
            a.wrap(c);
            a.wrap(e);
            c = a.closest("div");
            e = a.closest("span");
            a.is(":disabled") && c.addClass(b.disabledClass);
            c.bind("mouseenter.uniform",
                function () {
                    c.addClass(b.hoverClass)
                }).bind("mouseleave.uniform", function () {
                    c.removeClass(b.hoverClass);
                    c.removeClass(b.activeClass)
                }).bind("mousedown.uniform touchbegin.uniform", function () {
                    c.addClass(b.activeClass)
                }).bind("mouseup.uniform touchend.uniform", function () {
                    c.removeClass(b.activeClass)
                }).bind("click.uniform touchend.uniform", function (b) {
                    d(b.target).is("span, div") && (a[0].dispatchEvent ? (b = document.createEvent("MouseEvents"), b.initEvent("click", !0, !0), a[0].dispatchEvent(b)) : a.click())
                });
            a.bind("focus.uniform",
                function () {
                    c.addClass(b.focusClass)
                }).bind("blur.uniform", function () {
                    c.removeClass(b.focusClass)
                });
            d.uniform.noSelect(c);
            h(a)
        }

        function k(a) {
            var c = d("<div />"),
                e = d("<span />"),
                f = a.width();
            "none" == a.css("display") && b.autoHide && c.hide();
            c.addClass(b.selectClass);
            if (b.selectAutoWidth) {
                var g = c.width(),
                    l = e.width() - f;
                c.width(g - l + 25);
                a.width(f + 32);
                a.css("left", "2px");
                e.width(f)
            }
            b.useID && a.attr("id") && c.attr("id", b.idPrefix + "-" + a.attr("id"));
            g = a.find(":selected:first");
            0 == g.length && (g = a.find("option:first"));
            e.html(g.html());
            a.css("opacity", 0);
            a.wrap(c);
            a.before(e);
            c = a.parent("div");
            e = a.siblings("span");
            b.selectAutoWidth && (g = parseInt(c.css("paddingLeft"), 10), e.width(f - g - 15), a.width(f + g), a.css("min-width", f + g + "px"), c.width(f + g));
            a.bind("change.uniform", function () {
                e.text(a.find(":selected").html());
                c.removeClass(b.activeClass)
            }).bind("focus.uniform", function () {
                c.addClass(b.focusClass)
            }).bind("blur.uniform", function () {
                c.removeClass(b.focusClass);
                c.removeClass(b.activeClass)
            }).bind("mousedown.uniform touchbegin.uniform",
                function () {
                    c.addClass(b.activeClass)
                }).bind("mouseup.uniform touchend.uniform", function () {
                    c.removeClass(b.activeClass)
                }).bind("click.uniform touchend.uniform", function () {
                    c.removeClass(b.activeClass)
                }).bind("mouseenter.uniform", function () {
                    c.addClass(b.hoverClass)
                }).bind("mouseleave.uniform", function () {
                    c.removeClass(b.hoverClass);
                    c.removeClass(b.activeClass)
                }).bind("keyup.uniform", function () {
                    e.text(a.find(":selected").html())
                });
            a.is(":disabled") && c.addClass(b.disabledClass);
            d.uniform.noSelect(e);
            h(a)
        }

        function m(a) {
            var c = d("<div />"),
                e = d("<span />");
            "none" == a.css("display") && b.autoHide && c.hide();
            c.addClass(b.checkboxClass);
            b.useID && a.attr("id") && c.attr("id", b.idPrefix + "-" + a.attr("id"));
            a.wrap(c);
            a.wrap(e);
            e = a.parent();
            c = e.parent();
            a.css("opacity", 0).bind("focus.uniform", function () {
                c.addClass(b.focusClass)
            }).bind("blur.uniform", function () {
                c.removeClass(b.focusClass)
            }).bind("click.uniform touchend.uniform", function () {
                a.is(":checked") ? (a.attr("checked", "checked"), e.addClass(b.checkedClass)) : (a.removeAttr("checked"),
                    e.removeClass(b.checkedClass))
            }).bind("mousedown.uniform touchbegin.uniform", function () {
                c.addClass(b.activeClass)
            }).bind("mouseup.uniform touchend.uniform", function () {
                c.removeClass(b.activeClass)
            }).bind("mouseenter.uniform", function () {
                c.addClass(b.hoverClass)
            }).bind("mouseleave.uniform", function () {
                c.removeClass(b.hoverClass);
                c.removeClass(b.activeClass)
            });
            a.is(":checked") && (a.attr("checked", "checked"), e.addClass(b.checkedClass));
            a.is(":disabled") && c.addClass(b.disabledClass);
            h(a)
        }

        function n(a) {
            var c =
                d("<div />"),
                e = d("<span />");
            "none" == a.css("display") && b.autoHide && c.hide();
            c.addClass(b.radioClass);
            b.useID && a.attr("id") && c.attr("id", b.idPrefix + "-" + a.attr("id"));
            a.wrap(c);
            a.wrap(e);
            e = a.parent();
            c = e.parent();
            a.css("opacity", 0).bind("focus.uniform", function () {
                c.addClass(b.focusClass)
            }).bind("blur.uniform", function () {
                c.removeClass(b.focusClass)
            }).bind("click.uniform touchend.uniform", function () {
                if (a.is(":checked")) {
                    var c = b.radioClass.split(" ")[0];
                    d("." + c + " span." + b.checkedClass + ":has([name='" + a.attr("name") +
                        "'])").removeClass(b.checkedClass);
                    e.addClass(b.checkedClass)
                } else e.removeClass(b.checkedClass)
            }).bind("mousedown.uniform touchend.uniform", function () {
                a.is(":disabled") || c.addClass(b.activeClass)
            }).bind("mouseup.uniform touchbegin.uniform", function () {
                c.removeClass(b.activeClass)
            }).bind("mouseenter.uniform touchend.uniform", function () {
                c.addClass(b.hoverClass)
            }).bind("mouseleave.uniform", function () {
                c.removeClass(b.hoverClass);
                c.removeClass(b.activeClass)
            });
            a.is(":checked") && e.addClass(b.checkedClass);
            a.is(":disabled") && c.addClass(b.disabledClass);
            h(a)
        }

        function o(a) {
            var c = d("<div />"),
                e = d("<span>" + b.fileDefaultText + "</span>"),
                f = d("<span>" + b.fileBtnText + "</span>");
            "none" == a.css("display") && b.autoHide && c.hide();
            c.addClass(b.fileClass);
            e.addClass(b.filenameClass);
            f.addClass(b.fileBtnClass);
            b.useID && a.attr("id") && c.attr("id", b.idPrefix + "-" + a.attr("id"));
            a.wrap(c);
            a.after(f);
            a.after(e);
            c = a.closest("div");
            e = a.siblings("." + b.filenameClass);
            f = a.siblings("." + b.fileBtnClass);
            a.attr("size") || a.attr("size",
                c.width() / 10);
            var g = function () {
                var c = a.val();
                "" === c ? c = b.fileDefaultText : (c = c.split(/[\/\\]+/), c = c[c.length - 1]);
                e.text(c)
            };
            g();
            a.css("opacity", 0).bind("focus.uniform", function () {
                c.addClass(b.focusClass)
            }).bind("blur.uniform", function () {
                c.removeClass(b.focusClass)
            }).bind("mousedown.uniform", function () {
                a.is(":disabled") || c.addClass(b.activeClass)
            }).bind("mouseup.uniform", function () {
                c.removeClass(b.activeClass)
            }).bind("mouseenter.uniform", function () {
                c.addClass(b.hoverClass)
            }).bind("mouseleave.uniform",
                function () {
                    c.removeClass(b.hoverClass);
                    c.removeClass(b.activeClass)
                });
            d.browser.msie ? a.bind("click.uniform.ie7", function () {
                setTimeout(g, 0)
            }) : a.bind("change.uniform", g);
            a.is(":disabled") && c.addClass(b.disabledClass);
            d.uniform.noSelect(e);
            d.uniform.noSelect(f);
            h(a)
        }

        function h(a) {
            a.data("uniformed", "true");
            elem = a.get();
            1 < elem.length ? d.each(elem, function (a, b) {
                d.uniform.elements.push(b)
            }) : d.uniform.elements.push(elem)
        }
        var b = d.extend({}, d.uniform.options, b),
            p = this;
        !1 !== b.resetSelector && d(b.resetSelector).mouseup(function () {
            window.setTimeout(function () {
                d.uniform.update(p)
            },
                10)
        });
        d.uniform.restore = function (a) {
            a == i && (a = d(d.uniform.elements));
            d(a).each(function () {
                if (d(this).data("uniformed")) {
                    d(this).is(":checkbox") ? d(this).unwrap().unwrap() : d(this).is("select") ? (d(this).siblings("span").remove(), d(this).unwrap()) : d(this).is(":radio") ? d(this).unwrap().unwrap() : d(this).is(":file") ? (d(this).siblings("span").remove(), d(this).unwrap()) : d(this).is("button, :submit, :reset, a, input[type='button']") && d(this).unwrap().unwrap();
                    d(this).unbind(".uniform");
                    d(this).css("opacity",
                        "1");
                    var b = d.inArray(d(a), d.uniform.elements);
                    d.uniform.elements.splice(b, 1);
                    d(this).removeData("uniformed")
                }
            })
        };
        d.uniform.noSelect = function (a) {
            function b() {
                return !1
            }
            d(a).each(function () {
                this.onselectstart = this.ondragstart = b;
                d(this).mousedown(b).css({
                    MozUserSelect: "none"
                })
            })
        };
        d.uniform.update = function (a) {
            a == i && (a = d(d.uniform.elements));
            a = d(a);
            a.each(function () {
                var a = d(this);
                if (a.data("uniformed"))
                    if (a.is("select")) {
                        var e = a.siblings("span"),
                            f = a.parent("div");
                        f.removeClass(b.hoverClass + " " + b.focusClass +
                            " " + b.activeClass);
                        e.html(a.find(":selected").html());
                        a.is(":disabled") ? f.addClass(b.disabledClass) : f.removeClass(b.disabledClass)
                    } else a.is(":checkbox") ? (e = a.closest("span"), f = a.closest("div"), f.removeClass(b.hoverClass + " " + b.focusClass + " " + b.activeClass), e.removeClass(b.checkedClass), a.is(":checked") && e.addClass(b.checkedClass), a.is(":disabled") ? f.addClass(b.disabledClass) : f.removeClass(b.disabledClass)) : a.is(":radio") ? (e = a.closest("span"), f = a.closest("div"), f.removeClass(b.hoverClass + " " + b.focusClass +
                        " " + b.activeClass), e.removeClass(b.checkedClass), a.is(":checked") && e.addClass(b.checkedClass), a.is(":disabled") ? f.addClass(b.disabledClass) : f.removeClass(b.disabledClass)) : a.is(":file") ? (f = a.parent("div"), e = a.siblings("." + b.filenameClass), btnTag = a.siblings(b.fileBtnClass), f.removeClass(b.hoverClass + " " + b.focusClass + " " + b.activeClass), e.text(a.val()), a.is(":disabled") ? f.addClass(b.disabledClass) : f.removeClass(b.disabledClass)) : a.is(":submit, :reset, button, a, input[type='button']") && (f = a.closest("div"),
                        f.removeClass(b.hoverClass + " " + b.focusClass + " " + b.activeClass), a.is(":disabled") ? f.addClass(b.disabledClass) : f.removeClass(b.disabledClass))
            })
        };
        return this.each(function () {
            var a = d(this);
            !a.data("uniformed") && d.support.selectOpacity && (a.is("select") ? this.multiple || (a.attr("size") == i || 1 >= a.attr("size")) && k(a) : a.is(":checkbox") ? m(a) : a.is(":radio") ? n(a) : a.is(":file") ? o(a) : a.is(":text, :password, input[type='email'], input[type='search'], input[type='tel'], input[type='url'], input[type='datetime'], input[type='date'], input[type='month'], input[type='week'], input[type='time'], input[type='datetime-local'], input[type='number'], input[type='color']") ?
                (a.addClass(a.attr("type")), h(a)) : a.is("textarea") ? (a.addClass("uniform"), h(a)) : a.is("a, :submit, :reset, button, input[type='button']") && j(a))
        })
    }
})(jQuery);
