/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable import/no-unresolved */
/**
 * Date manipulator.
 */

import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';

const dayJS = dayjs.extend(utc);

export default {
  getFullDateLocal(dateString: string | boolean | undefined): string {
    return dayJS
      .utc(dateString as boolean)
      .local()
      .format('MMM DD, YYYY HH:mm');
  },
  getFullLocalChatTime(dateString: string | boolean | undefined): string {
    return dayJS
      .utc(dateString as boolean)
      .local()
      .format('HH:mm, MMM DD YYYY');
  },
};
