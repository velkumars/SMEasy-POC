        var logger = {
            error: error,
            info: info,
            //log: log,  // straight to console; bypass toast
            success: success,
            warning: warning
        };

        function error(message, title) {
            toastr.error(message, title);
            $log.error("Error: " + message);
        }

        function info(message, title) {
            toastr.info(message, title);
            $log.info("Info: " + message);
        }

        function log(message) {
            $log.log(message);
        }

        function success(message, title) {
            toastr.success(message, title);
            $log.info("Success: " + message);
        }

        function warning(message, title) {
            toastr.warning(message, title);
            $log.warn("Warning: " + message);
        }

        toastr.options.timeOut = 3000; // 2 second toast timeout
        toastr.options.positionClass = 'toast-top-right';