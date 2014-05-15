--
-- Table structure for table `cards`
--

CREATE TABLE IF NOT EXISTS `cards` (
  `cardid` int(2) NOT NULL AUTO_INCREMENT,
  `card` varchar(4) NOT NULL,
  PRIMARY KEY (`cardid`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=53 ;

--
-- Dumping data for table `cards`
--

INSERT INTO `cards` (`cardid`, `card`) VALUES
(1, 'A.H'),
(2, 'A.D'),
(3, 'A.C'),
(4, 'A.S'),
(5, 'K.H'),
(6, 'K.D'),
(7, 'K.C'),
(8, 'K.S'),
(9, 'Q.H'),
(10, 'Q.D'),
(11, 'Q.C'),
(12, 'Q.S'),
(13, 'J.H'),
(14, 'J.D'),
(15, 'J.C'),
(16, 'J.S'),
(17, '10.H'),
(18, '10.D'),
(19, '10.C'),
(20, '10.S'),
(21, '9.H'),
(22, '9.D'),
(23, '9.C'),
(24, '9.S'),
(25, '8.H'),
(26, '8.D'),
(27, '8.C'),
(28, '8.S'),
(29, '7.H'),
(30, '7.D'),
(31, '7.C'),
(32, '7.S'),
(33, '6.H'),
(34, '6.D'),
(35, '6.C'),
(36, '6.S'),
(37, '5.H'),
(38, '5.D'),
(39, '5.C'),
(40, '5.S'),
(41, '4.H'),
(42, '4.D'),
(43, '4.C'),
(44, '4.S'),
(45, '3.H'),
(46, '3.D'),
(47, '3.C'),
(48, '3.S'),
(49, '2.H'),
(50, '2.D'),
(51, '2.C'),
(52, '2.S');

-- --------------------------------------------------------
