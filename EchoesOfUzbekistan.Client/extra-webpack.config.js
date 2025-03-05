module.exports = {
    module: {
      rules: [
        {
          test   : /\.less$/,
          loader: 'less-loader',
          options: {
            lessOptions: {
                modifyVars: { // modify theme variable
                    'primary-color': '#3cc37e',
                    'height-base': '50px',
                    'processing-color': '#3cc37e'
                  },
                  javascriptEnabled: true
            }
          }
        }
      ]
    }
  };