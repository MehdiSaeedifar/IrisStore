///////////////////////////////////////////////////
/////  Copyright 2013 Rahmat.Rezaei@yahoo.com /////
///////////////////////////////////////////////////
var PersianDatePicker = function () {
    var _shanbeh = "شنبه";
    var _dayName = new Array(_shanbeh, 'یک' + _shanbeh, 'دو' + _shanbeh, 'سه' + _shanbeh, 'چهار' + _shanbeh, 'پنج' + _shanbeh, 'جمعه');
    var _monthName = new Array('فروردین', 'اردیبهشت', 'خرداد', 'تیر', 'مرداد', 'شهریور', 'مهر', 'آبان', 'آذر', 'دی', 'بهمن', 'اسفند');
    var _today = null;
    var _datePicker = null;
    var _textBox = null;
    var _datePickerStyle = null;
    var _clicked = false;

    function init() {
        _datePicker = createElement("div", document.body);
        _datePickerStyle = _datePicker.style;
        _datePickerStyle.position = "absolute";
        _datePickerStyle.zIndex = 10000000;
        _datePicker.onmousedown = function () {
            _clicked = true;
        };
        _datePicker.onclick = function () {
            _textBox.focus();
        };
    }

    function hide() {
        _datePickerStyle.visibility = "hidden";
    }

    this.show = function (textBox, today) {
        if (_datePicker == null) {
            init();
        }
        _today = today;
        _textBox = textBox;
        _textBox.onblur = function () {
            if (!_clicked) {
                hide();
            }
            _clicked = false;
        };

        var left = 0;
        var top = 0;
        var parent = _textBox;
        while (parent.offsetParent) {
            left += parent.offsetLeft;
            top += parent.offsetTop;
            parent = parent.offsetParent;
        }
        _datePickerStyle.left = left + "px";
        _datePickerStyle.top = top + _textBox.offsetHeight + "px";
        _datePickerStyle.visibility = "visible";

        draw(_textBox.value.length == 10 ? _textBox.value : _today);
    }

    function setValue(date) {
        _textBox.value = date;
        _textBox.focus();
        hide();
        try {
            _textBox.onchange();
        }
        catch (ex) {

        }
    }

    function changeDay(date, day) {
        return date.substring(0, 8) + (day < 10 ? "0" : "") + day;
    }

    function createElement(tag, parent) {
        var element = document.createElement(tag);
        parent.appendChild(element);
        return element;
    }

    function draw(date) {
        _textBox.focus();
        var weekDay = getWeekDay(date.substring(0, 8) + "01");

        setInnerHTML(_datePicker, "");

        var table = createElement("table", _datePicker);
        setClassName(table, "datePicker");
        table.cellSpacing = 0;

        var tr = table.insertRow(0);
        setClassName(tr, "datePickerHeader");

        var td = createElement("td", tr)
        td.colSpan = 3;

        var button = createElement("button", td)
        setInnerHTML(button, "&lt;");
        button.onclick = function () { draw(previousMonth(date)); };

        var span = createElement("span", td)
        setInnerHTML(span, _monthName[date.substring(5, 7) - 1]);
        setClassName(span, "datePickerMonth");

        button = createElement("button", td)
        setInnerHTML(button, "&gt;");
        button.onclick = function () { draw(nextMonth(date)); };

        td = createElement("td", tr)
        td.colSpan = 4;
        td.style.textAlign = "left";

        button = createElement("button", td)
        setInnerHTML(button, "&lt;");
        button.onclick = function () { draw(previousYear(date)); };

        span = createElement("span", td)
        setInnerHTML(span, date.substring(0, 4));
        setClassName(span, "datePickerYear");

        button = createElement("button", td)
        setInnerHTML(button, "&gt;");
        button.onclick = function () { draw(nextYear(date)); };

        for (var row = 0; row < 7; row++) {
            tr = table.insertRow(row + 1);
            if (row == 6)
                setClassName(tr, "datePickerFriDay");
            else if (mod(row, 2) != 1)
                setClassName(tr, "datePickerRow");
            td = createElement("td", tr)
            setInnerHTML(td, _dayName[row]);
            for (var col = 0; col < 6; col++) {
                var cellValue = col * 7 + row - weekDay + 1;
                td = createElement("td", tr)
                if (cellValue > 0 && cellValue <= getMonthDays(date)) {
                    setInnerHTML(td, cellValue);
                    var cellDate = changeDay(date, cellValue);
                    var cellClassName = "datePickerDay";
                    if (cellDate == _textBox.value)
                        cellClassName = "datePickerDaySelect";
                    else if (cellDate == _today)
                        cellClassName = "datePickerToday";
                    setClassName(td, cellClassName);
                    td.onclick = function (event) {
                        event.stopPropagation();
                        setValue(changeDay(date, this.innerHTML));
                    };
                }
            }
        }

        tr = table.insertRow(8);
        setClassName(tr, "datePickerFooter");

        td = createElement("td", tr)
        td.colSpan = 2;

        button = createElement("button", td)
        setInnerHTML(button, "امروز");
        button.onclick = function (event) {
            event.stopPropagation();
            setValue(_today);
        };

        td = createElement("td", tr)
        td.colSpan = 5;
        td.style.textAlign = "left";

        button = createElement("button", td)
        setInnerHTML(button, "خالی");
        button.onclick = function (event) {
            event.stopPropagation();
            setValue('');
        };
    }

    function nextYear(date) {
        var dateArray = date.split('/');
        return (toInt(dateArray[0]) + 1).toString() + "/" + dateArray[1] + "/" + dateArray[2];
    }

    function previousYear(date) {
        var dateArray = date.split('/');
        return (toInt(dateArray[0]) - 1).toString() + "/" + dateArray[1] + "/" + dateArray[2];
    }

    function nextMonth(date) {
        var dateArray = date.split('/');

        if (dateArray[1] < 9)
            return dateArray[0] + "/0" + (toInt(dateArray[1].substring(1, 2)) + 1).toString() + "/" + dateArray[2];
        if (dateArray[1] == 9)
            return dateArray[0] + "/10/" + dateArray[2];
        if (dateArray[1] < 12)
            return dateArray[0] + "/" + (toInt(dateArray[1]) + 1).toString() + "/" + dateArray[2];
        return (toInt(dateArray[0]) + 1).toString() + "/01/" + dateArray[2];
    }

    function previousMonth(date) {
        var dateArray = date.split('/');
        if (dateArray[1] > 10)
            return dateArray[0] + "/" + (toInt(dateArray[1]) - 1).toString() + "/" + dateArray[2];
        if (dateArray[1] > 1)
            return dateArray[0] + "/0" + (dateArray[1] - 1).toString() + "/" + dateArray[2];
        return (toInt(dateArray[0]) - 1).toString() + "/12/" + dateArray[2];
    }

    function isLeapYear(year) {
        return (((((year - 474) % 2820) + 512) * 682) % 2816) < 682;
    }

    function mod(a, b) {
        return Math.abs(a - (b * Math.floor(a / b)));
    }

    function getWeekDay(date) {
        return mod(getDiffDays('1392/03/25', date), 7);
    }

    function getDiffDays(date1, date2) {
        var diffDays = getDays(date2) - getDays(date1);
        var dateArray1 = date1.split('/');
        var dateArray2 = date2.split('/');
        var y1 = (dateArray1[0] < dateArray2[0]) ? dateArray1[0] : dateArray2[0];
        var y2 = (dateArray1[0] < dateArray2[0]) ? dateArray2[0] : dateArray1[0];
        for (var y = y1; y < y2; y++)
            if (isLeapYear(y))
                diffDays += (dateArray1[0] < dateArray2[0]) ? 366 : -366;
            else
                diffDays += (dateArray1[0] < dateArray2[0]) ? 365 : -365;
        return toInt(diffDays);
    }

    function setInnerHTML(element, html) {
        element.innerHTML = html;
    }

    function setClassName(element, className) {
        element.className = className;
    }

    function toInt(text) {
        return parseInt(text, 10);
    }

    function getDays(date) {
        var dateArray = date.split('/');
        if (dateArray[1] < 8)
            return (dateArray[1] - 1) * 31 + toInt(dateArray[2]);
        return 6 * 31 + (dateArray[1] - 7) * 30 + toInt(dateArray[2]);
    }

    function getMonthDays(date) {
        var dateArray = date.split('/');
        if (dateArray[1] < 7)
            return 31;
        if (dateArray[1] < 12)
            return 30;
        return isLeapYear(dateArray[0]) ? 30 : 29;
    }
}
var _persianDatePicker = new PersianDatePicker();

PersianDatePicker.Show = function (textBox, today) {
    _persianDatePicker.show(textBox, today);
};